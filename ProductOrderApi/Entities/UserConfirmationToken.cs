namespace ProductOrderApi.Entities
{
    internal class UserConfirmationToken : EntityBase
    {
        public int UserId { get; set; }
        public Guid Token { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
