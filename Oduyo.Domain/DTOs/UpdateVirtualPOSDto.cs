namespace Oduyo.Domain.DTOs
{
    public class UpdateVirtualPOSDto
    {
        public string Name { get; set; }
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public bool IsActive { get; set; }
    }
}