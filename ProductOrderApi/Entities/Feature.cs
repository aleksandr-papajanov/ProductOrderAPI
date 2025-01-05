using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductOrderApi.Entities
{
    [Table("Features")]
    internal class Feature : EntityBase
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public required string Name { get; set; }
    }
}
