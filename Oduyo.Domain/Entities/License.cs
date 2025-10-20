using Oduyo.Domain.Enums;

namespace Oduyo.Domain.Entities
{
    public class License : EntityBase
    {
        public int CompanyId { get; set; }
        public int? ProductId { get; set; }
        public int? PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LicensePeriod Period { get; set; }
        public bool IsTrial { get; set; }
        public bool IsActive { get; set; }

        // NEW - Phase 1 additions
        public int? TrialDays { get; set; }
        public int GracePeriodDays { get; set; } = 7;
        public DateTime? GracePeriodStartDate { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual Product Product { get; set; }
        public virtual Package Package { get; set; }
    }
}
