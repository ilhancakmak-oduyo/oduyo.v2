namespace Oduyo.Infrastructure.Interfaces
{
    public interface IOfferPricingService
    {
        Task<OfferPriceCalculation> CalculatePricingAsync(OfferPricingRequest request);
        Task<bool> ValidateCampaignEligibilityAsync(int campaignId, IEnumerable<int> packageIds);
        Task<decimal> CalculateCrossSellPerksAsync(IEnumerable<OfferDetailDto> details);
    }

    public class OfferPricingRequest
    {
        public IEnumerable<OfferDetailDto> Details { get; set; }
        public int? CampaignId { get; set; }
        public decimal ManualDiscountRate { get; set; }
        public int UserId { get; set; }
    }

    public class OfferDetailDto
    {
        public int? PackageId { get; set; }
        public int? ModuleId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OfferPriceCalculation
    {
        public decimal SubTotal { get; set; }
        public decimal CampaignDiscount { get; set; }
        public decimal CrossSellDiscount { get; set; }
        public decimal ManualDiscount { get; set; }
        public decimal FinalPrice { get; set; }
        public bool IsCampaignActive { get; set; }
        public bool IsCampaignFrozen { get; set; }
    }
}
