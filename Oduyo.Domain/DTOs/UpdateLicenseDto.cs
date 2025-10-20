using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class UpdateLicenseDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LicensePeriod Period { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrialPeriod { get; set; }
    }
}