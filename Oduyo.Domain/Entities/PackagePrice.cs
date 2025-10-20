namespace Oduyo.Domain.Entities
{
    public class PackagePrice : EntityBase
    {
        public int PackageId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive => !EffectiveTo.HasValue || EffectiveTo.Value > DateTime.UtcNow;
    }
}