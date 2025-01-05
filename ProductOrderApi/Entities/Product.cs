using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductOrderApi.Entities
{
    [Table("Products")]
    internal class Product : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{6}$", ErrorMessage = "Product code should consist of 2 letters followed by 6 digits")]
        public required string Code { get; set; }

        [MaxLength(256)]
        public required string Name { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool IsAvailable { get; set; } = true;


        public List<ProductFeature> Features { get; } = new();
        public List<OrderProduct> Orders { get; } = new();
    }
}
