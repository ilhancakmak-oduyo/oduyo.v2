using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}