using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("Users")]
    internal class User : EntityBase
    {
        [Key]
        public int Id { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public bool IsActive { get; set; } = false;

        public bool IsGoogleUser { get; set; } = false;


        public List<UserRole> Roles { get; } = new();
        public List<Order> Orders { get; } = new();
        public UserConfirmationToken ConfirmationToken { get; set; } = null!;

        public bool IsInRole(string roleName)
        {
            foreach (var role in Roles)
            {
                if (role.Role.Equals(roleName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
