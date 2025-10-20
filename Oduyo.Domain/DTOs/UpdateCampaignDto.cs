namespace Oduyo.Domain.DTOs
{
    public class UpdateCampaignDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountRate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}