using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("ProductFeatures")]
    internal class ProductFeature : EntityBase
    {
        public int ProductId { get; set; }
        public int FeatureId { get; set; }

        [MaxLength(256)]
        public required string Value { get; set; }


        public Product Product { get; set; } = null!;
        public Feature Feature { get; set; } = null!;
    }
}
