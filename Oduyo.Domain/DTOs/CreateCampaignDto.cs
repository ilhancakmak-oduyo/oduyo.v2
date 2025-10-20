namespace Oduyo.Domain.DTOs
{
    public class CreateCampaignDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountRate { get; set; }
        public string Description { get; set; }
    }
}