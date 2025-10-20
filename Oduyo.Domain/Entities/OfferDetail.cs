namespace Oduyo.Domain.Entities
{
    public class OfferDetail : EntityBase
    {
        public int OfferId { get; set; }
        public int? ProductId { get; set; }
        public int? PackageId { get; set; }
        public int? ModuleId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal Total { get; set; }
    }
}