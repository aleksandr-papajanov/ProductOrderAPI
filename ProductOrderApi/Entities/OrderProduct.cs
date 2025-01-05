using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("OrderProducts")]
    internal class OrderProduct : EntityBase
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }


        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
