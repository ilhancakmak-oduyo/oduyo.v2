namespace Oduyo.Domain.DTOs
{
    public class CreateOfferDetailDto
    {
        public int? ProductId { get; set; }
        public int? PackageId { get; set; }
        public int? ModuleId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountRate { get; set; }
    }
}