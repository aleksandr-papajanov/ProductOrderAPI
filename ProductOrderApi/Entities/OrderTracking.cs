using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("OrderTracking")]
    internal class OrderTracking : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Range(1, 8)]
        public OrderStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Order Order { get; set; } = null!;
    }
}
