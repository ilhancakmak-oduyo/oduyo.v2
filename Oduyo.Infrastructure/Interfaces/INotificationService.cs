using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(CreateNotificationDto dto);
        Task<bool> MarkAsReadAsync(int notificationId, int userId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
    }
}