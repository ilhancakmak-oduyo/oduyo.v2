namespace Oduyo.Domain.Entities
{
    public class AuditLog : EntityBase
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public int EntityId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string IpAddress { get; set; }
    }
}
