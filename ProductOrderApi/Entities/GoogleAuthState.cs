using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("GoogleAuthStates")]
    internal class GoogleAuthState : EntityBase
    {
        public Guid State { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string RequestType { get; set; } = "Login";
    }
}
