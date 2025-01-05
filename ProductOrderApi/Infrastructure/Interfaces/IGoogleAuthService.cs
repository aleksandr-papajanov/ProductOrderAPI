using MySqlX.XDevAPI;
using Newtonsoft.Json;
using ProductOrderApi.Helpers;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IGoogleAuthService
    {
        Task<string> CreateRedirectUrlAsync(string requestType);
        Task<GoogleToken> GetTokenAsync(string code, Guid state);
        Task VerifyTokenAsync(GoogleToken token);
        Task<GoogleUserInfo> GetUserInfoAsync(GoogleToken token);
    }
}
