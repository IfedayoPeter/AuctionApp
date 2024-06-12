using AuctionApp.Domain.Entities.Notification;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface INotificationService
    {
        Task<Result<Notification>> CreateNotification(string Message,  string roomCode);
        Task<Result<Notification>> CreateBidNotification(string Message,  string roomCode);
        Task<Result<List<Notification>>> GetUserNotifications(string UserCode);
        Task<Result<bool>> MarkAsRead(long Id);
    }
}
