namespace Oduyo.Domain.Entities
{
    public class TenantUser : EntityBase
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }
    }
}