using ProductOrderApi.DTOs.Users;
using ProductOrderApi.Entities;

namespace ProductOrderApi.DTOs.Mappers
{
    internal static class UserMapper
    {
        public static User ToEntity(this RegisterRequest request)
        {
            return new User
            {
                Email = request.Email,
                Password = request.Password,
                IsActive = false
            };
        }
    }
}
