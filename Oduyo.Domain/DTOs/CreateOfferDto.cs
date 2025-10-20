using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateOfferDto
    {
        public int CompanyId { get; set; }
        public int? DealerId { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public int CurrencyId { get; set; }
        public PriceType PriceType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal ManualDiscount { get; set; }
        public decimal ManualDiscountRate { get; set; }
        public string Description { get; set; }
        public List<CreateOfferDetailDto> Details { get; set; }
    }
}