using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace ProductOrderApi.Entities
{
    [Table("UserRoles")]
    internal class UserRole : EntityBase
    {
        public int UserId { get; set; }
        public required string Role { get; set; }

        public User User { get; set; } = null!;
    }
}
