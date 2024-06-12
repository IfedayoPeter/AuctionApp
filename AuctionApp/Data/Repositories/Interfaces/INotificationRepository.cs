using AuctionApp.Domain.Entities.Notification;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotification(Notification notification);
        Task<List<Notification>> GetUserNotifications(string userCode);
        Task<bool> MarkAsRead(long notificationId);
    }
}
