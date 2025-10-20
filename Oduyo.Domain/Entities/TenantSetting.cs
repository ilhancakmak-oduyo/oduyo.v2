namespace Oduyo.Domain.Entities
{
    public class TenantSetting : EntityBase
    {
        public int TenantId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}