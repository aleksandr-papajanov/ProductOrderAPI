using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ProductOrderApi.Services
{
    internal class UserService : IUserService
    {
        private readonly string jwtKey;
        private readonly string jwtIssuer;
        private readonly string jwtAudience;

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<UserConfirmationToken> _userConfirmationTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<UserConfirmationToken> userConfirmationTokenRepository)
        {
            jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
            jwtIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
            jwtAudience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");

            _httpContextAccessor = httpContextAccessor;

            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userConfirmationTokenRepository = userConfirmationTokenRepository;
        }


        public async Task<User> GetUserAsync(int id)
        {
            var user = await _userRepository.All
                .Include(e => e.Roles)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new EntityNotFoundApiException($"User with id {id} is not found");
            }

            return user;
        }

        public async Task<User> GetUserFromClaimAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return await GetUserAsync(userId);
            }

            throw new EntityNotFoundApiException($"User with id specified in token is not found");
        }


        public async Task<User> GetUserAsync(string email, string password)
        {
            var user = await _userRepository.All
                .Include(e => e.Roles)
                .Where(e => e.Email.Equals(email))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new EntityNotFoundApiException($"User with email {email} is not found");
            }

            if (!user.IsActive)
            {
                throw new AccessDeniedApiException();
            }

            if (!PasswordHelper.VerifyPassword(user.Password, password))
            {
                throw new ServiceLayerApiException("Invalid credentials", HttpStatusCode.BadRequest);
            }

            return user;
        }

        public string CreateToken(User user, DateTime expires)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task AddUserAsync(User user)
        {
            var exists = await _userRepository.All
                .Where(e => e.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
                .AnyAsync();

            if (exists)
            {
                throw new EntityAlreadyExistsApiException("User with the specified email already exists");
            }

            user.Password = PasswordHelper.HashPassword(user.Password);

            await _userRepository.AddAsync(user);

            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = user.Id,
                Role = "Customer"
            });

            await _userConfirmationTokenRepository.AddAsync(new UserConfirmationToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            });
        }

        public async Task ConfirmEmail(Guid token)
        {
            var confirmation = await _userConfirmationTokenRepository.All
                .Include(e => e.User)
                .Where(e => e.Token.Equals(token))
                .FirstOrDefaultAsync();

            if (confirmation == null)
            {
                throw new EntityNotFoundApiException("Invalid confirmation token");
            }

            if (DateTime.Now > confirmation.CreatedAt.AddMinutes(30))
            {
                await _userConfirmationTokenRepository.DeleteAsync(confirmation);
                await _userRepository.DeleteAsync(confirmation.User);

                throw new ServiceLayerApiException("Confirmation token is expired", HttpStatusCode.Conflict);
            }

            if (confirmation.User == null)
            {
                throw new EntityNotFoundApiException($"User connected with confirmation token is not found");
            }

            confirmation.User.IsActive = true;

            await _userRepository.UpdateAsync(confirmation.User);
            await _userConfirmationTokenRepository.DeleteAsync(confirmation);
        }

        public async Task<User> AddGoogleUserAsync(GoogleUserInfo userInfo)
        {
            var user = new User
            {
                Email = userInfo.Email,
                Password = "",
                IsActive = true,
                IsGoogleUser = true
            };

            var exists = await _userRepository.All
                .Where(e => e.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
                .AnyAsync();

            if (exists)
            {
                throw new ServiceLayerApiException("User with the specified email already exists", HttpStatusCode.Conflict);
            }

            await _userRepository.AddAsync(user);

            var role = new UserRole
            {
                UserId = user.Id,
                Role = "Customer"
            };

            await _userRoleRepository.AddAsync(role);

            return user;
        }

        public async Task<User> GetGoogleUserAsync(string email)
        {
            var user = await _userRepository.All
                    .Include(e => e.Roles)
                    .Where(e => e.Email.Equals(email) && e.IsGoogleUser)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new EntityNotFoundApiException($"User with the specified email is not found");
            }

            if (!user.IsActive)
            {
                throw new AccessDeniedApiException();
            }

            return user;
        }
    }
}
