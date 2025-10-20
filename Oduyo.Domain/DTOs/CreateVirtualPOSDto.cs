namespace Oduyo.Domain.DTOs
{
    public class CreateVirtualPOSDto
    {
        public string Name { get; set; }
        public string MerchantId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}