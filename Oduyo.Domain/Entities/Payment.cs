using Oduyo.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oduyo.Domain.Entities
{
    public class Payment : EntityBase
    {
        public int CompanyId { get; set; }
        public int? OfferId { get; set; }
        public int? VirtualPOSId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        [StringLength(100)]
        public string ProviderTransactionId { get; set; }

        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }

        // NEW - Phase 1
        public DateTime? CompletedAt { get; set; }

        [StringLength(500)]
        public string FailureReason { get; set; }

        [StringLength(50)]
        public string FailureCode { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual Offer Offer { get; set; }
        public virtual VirtualPOS VirtualPOS { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
