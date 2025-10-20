namespace Oduyo.Domain.DTOs
{
    public class CreatePaymentDto
    {
        public int CompanyId { get; set; }
        public int? OfferId { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public int? VirtualPOSId { get; set; }
    }
}