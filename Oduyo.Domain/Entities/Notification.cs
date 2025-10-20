using Oduyo.Domain.Enums;

namespace Oduyo.Domain.Entities
{
    public class Notification : EntityBase
    {
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string EntityType { get; set; }
        public int? EntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
