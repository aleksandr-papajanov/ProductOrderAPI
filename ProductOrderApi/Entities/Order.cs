using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("Orders")]
    internal class Order : EntityBase
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(1024)]
        public string? Comment { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal TotalPrice { get; set; }


        public User User { get; set; } = null!;
        public List<OrderProduct> Cart { get; } = new();
        public List<OrderTracking> Tracking { get; set; } = new();
    }
}
