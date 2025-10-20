using Oduyo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Oduyo.Domain.Entities
{
    public class Company : EntityBase
    {
        public int? ChannelId { get; set; }
        public int? DealerId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public CompanyStatus Status { get; set; }
        public int? SupportUserId { get; set; }
        public string ReferenceCode { get; set; }
        public string Notes { get; set; }

        // NEW - Phase 1 additions
        [Required]
        [StringLength(200)]
        public string ContactName { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string ProductInterests { get; set; }  // Comma-separated

        // Navigation properties
        public virtual Channel Channel { get; set; }
        public virtual Dealer Dealer { get; set; }
        public virtual User SupportUser { get; set; }
    }
}