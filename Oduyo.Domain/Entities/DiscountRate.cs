using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oduyo.Domain.Entities
{
    public class DiscountRate : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }  // Bronze, Silver, Gold

        [StringLength(200)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal MinRate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal MaxRate { get; set; }

        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
