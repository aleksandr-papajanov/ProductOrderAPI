using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using System.Security.Claims;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IUserService
    {
        Task AddUserAsync(User user);
        Task<User> AddGoogleUserAsync(GoogleUserInfo userInfo);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string email, string password);
        Task<User> GetGoogleUserAsync(string email);
        Task<User> GetUserFromClaimAsync();
        Task ConfirmEmail(Guid token);
        string CreateToken(User user, DateTime expires);
    }
}
