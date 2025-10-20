using Oduyo.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oduyo.Domain.Entities
{
    public class Offer : EntityBase
    {
        public string OfferNo { get; set; }
        public int CompanyId { get; set; }
        public int? DealerId { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public int CurrencyId { get; set; }
        public PriceType PriceType { get; set; }
        public OfferStatus Status { get; set; }
        public string Description { get; set; }
        public string RejectionReason { get; set; }

        // PRICING BREAKDOWN - Phase 1
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CampaignDiscount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ManualDiscount { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal ManualDiscountRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // CAMPAIGN - Phase 1
        public int? CampaignId { get; set; }
        public bool IsCampaignLocked { get; set; }  // 7-day freeze

        // OTP - Phase 1
        [StringLength(255)]
        public string OtpCodeHash { get; set; }  // Store hashed

        public DateTime? OtpSentAt { get; set; }
        public DateTime? OtpVerifiedAt { get; set; }
        public bool IsOtpVerified { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ICollection<OfferDetail> Details { get; set; }
    }
}