namespace Oduyo.Domain.DTOs
{
    public class CreateTenantSettingDto
    {
        public int TenantId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}