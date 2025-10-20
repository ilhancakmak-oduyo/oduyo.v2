using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oduyo.Domain.Entities
{
    public class Campaign : EntityBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsCurrentlyActive =>
            IsActive &&
            DateTime.UtcNow >= StartDate &&
            DateTime.UtcNow <= EndDate;

        [NotMapped]
        public bool IsWithinFreezePeriod =>
            DateTime.UtcNow >= EndDate.AddDays(-7) &&
            DateTime.UtcNow <= EndDate;

        // Navigation properties
        public virtual ICollection<CampaignPackage> CampaignPackages { get; set; }
        public virtual ICollection<CampaignModule> CampaignModules { get; set; }
    }
}