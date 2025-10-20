namespace Oduyo.Domain.Entities
{
    public class Dealer : EntityBase
    {
        public string Title { get; set; }
        public string CompanyName => Title; // Alias for backward compatibility
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}