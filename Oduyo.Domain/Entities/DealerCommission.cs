using Oduyo.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oduyo.Domain.Entities
{
    public class DealerCommission : EntityBase
    {
        public int DealerId { get; set; }
        public int CompanyId { get; set; }
        public int? OfferId { get; set; }
        public int? PaymentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseAmount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal CommissionRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CommissionAmount { get; set; }

        public CommissionStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        // Navigation properties
        public virtual Dealer Dealer { get; set; }
        public virtual Company Company { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
