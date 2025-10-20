namespace Oduyo.Domain.Entities
{
    public class ProductPrice : EntityBase
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}