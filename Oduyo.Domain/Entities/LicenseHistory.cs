namespace Oduyo.Domain.Entities
{
    public class LicenseHistory : EntityBase
    {
        public int LicenseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Action { get; set; }
        public DateTime? NewEndDate { get; set; }
        public string Notes { get; set; }
    }
}
