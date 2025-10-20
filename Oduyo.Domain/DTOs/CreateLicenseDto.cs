using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateLicenseDto
    {
        public int CompanyId { get; set; }
        public int? ProductId { get; set; }
        public int? PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LicensePeriod Period { get; set; }
        public bool IsTrialPeriod { get; set; }
    }
}