namespace Oduyo.Domain.Entities
{
    public class VirtualPOS : EntityBase
    {
        public string Name { get; set; }
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public bool IsActive { get; set; }
    }
}
