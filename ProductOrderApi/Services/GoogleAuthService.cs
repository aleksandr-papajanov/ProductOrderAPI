using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace ProductOrderApi.Services
{
    internal class GoogleAuthService : IGoogleAuthService
    {
        private readonly string authEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private readonly string tokenEndpoint = "https://oauth2.googleapis.com/token";
        private readonly string verifyTokenEndpoint = "https://www.googleapis.com/oauth2/v3/certs";
        private readonly string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri;

        private readonly IRepository<GoogleAuthState> _authStateRepository;

        public GoogleAuthService(IConfiguration congiguration, IRepository<GoogleAuthState> authStateRepository)
        {
            _authStateRepository = authStateRepository;

            clientId = congiguration["GoogleAuth:ClientId"] ?? throw new InvalidOperationException("GoogleAuth:ClientId is missing"); ;
            clientSecret = congiguration["GoogleAuth:ClientSecret"] ?? throw new InvalidOperationException("GoogleAuth:ClientSecret is missing"); ;
            redirectUri = congiguration["GoogleAuth:RedirectUri"] ?? throw new InvalidOperationException("GoogleAuth:RedirectUri is missing"); ;
        }

        public async Task<string> CreateRedirectUrlAsync(string requestType)
        {
            var state = new GoogleAuthState
            {
                State = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                RequestType = requestType
            };

            await _authStateRepository.AddAsync(state);

            var queryParams = HttpUtility.ParseQueryString(string.Empty);

            queryParams["client_id"] = clientId;
            queryParams["redirect_uri"] = redirectUri;
            queryParams["response_type"] = "code";
            queryParams["scope"] = "openid email profile";
            queryParams["state"] = state.State.ToString();

            return $"{authEndpoint}?{queryParams}";
        }

        public async Task<GoogleToken> GetTokenAsync(string code, Guid state)
        {
            var authState = await _authStateRepository.All
                .Where(e => e.State.Equals(state))
                .FirstOrDefaultAsync();

            if (authState == null)
            {
                throw new GoogleAuthentificationFailedApiException("Auth state hasn't been found");
            }

            if (authState.CreatedAt.AddMinutes(5) < DateTime.Now)
            {
                await _authStateRepository.DeleteAsync(authState);

                throw new GoogleAuthentificationFailedApiException("Auth state is expired");
            }

            using (var client = new HttpClient())
            {
                var tokenRequestParams = new Dictionary<string, string>
                {
                    { "code", code },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "redirect_uri", redirectUri },
                    { "grant_type", "authorization_code" }
                };

                var tokenResponse = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(tokenRequestParams));
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    throw new GoogleAuthentificationFailedApiException("Error while exchanging code for token: " + tokenResponse.ReasonPhrase);
                }

                var token = JsonConvert.DeserializeObject<GoogleToken>(tokenContent);

                if (token == null)
                {
                    throw new GoogleAuthentificationFailedApiException("Error while parsing google token response");
                }

                token.RequestType = authState.RequestType;

                await _authStateRepository.DeleteAsync(authState);

                return token;
            }
        }

        public async Task VerifyTokenAsync(GoogleToken token)
        {
            using (var client = new HttpClient())
            {
                var keysResponse = await client.GetStringAsync(verifyTokenEndpoint);

                if (JsonDocument.Parse(keysResponse).RootElement.TryGetProperty("keys", out JsonElement keys) &&
                    keys.ValueKind != JsonValueKind.Array)
                {
                    throw new GoogleAuthentificationFailedApiException("Invalid keys response");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://accounts.google.com",
                    ValidateAudience = true,
                    ValidAudience = clientId,
                    ValidateLifetime = true,
                    IssuerSigningKeys = keys.EnumerateArray().Select(key =>
                    {
                        var keyParams = new JsonWebKey(key.ToString());
                        return (SecurityKey)keyParams;
                    }).ToList()
                };

                try
                {
                    tokenHandler.ValidateToken(token.IdToken, validationParameters, out var validatedToken);
                }
                catch (Exception ex)
                {
                    throw new GoogleAuthentificationFailedApiException($"Invalid token: {ex.Message}");
                }
            }
        }

        public async Task<GoogleUserInfo> GetUserInfoAsync(GoogleToken token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");

                var response = await client.GetAsync(userInfoEndpoint);

                if (!response.IsSuccessStatusCode)
                {
                    throw new GoogleAuthentificationFailedApiException("Error while getting user info: " + response.ReasonPhrase);
                }

                var content = await response.Content.ReadAsStringAsync();

                var userInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(content);

                if (userInfo == null)
                {
                    throw new GoogleAuthentificationFailedApiException("Error while parsing google token responce");
                }

                return userInfo;
            }
        }

    }
}
