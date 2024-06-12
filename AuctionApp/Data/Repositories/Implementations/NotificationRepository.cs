using System.Text.Json;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Entities.Notification;
using AuctionApp.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;

        public NotificationRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
        }

        public async Task<Notification> CreateNotification(Notification Notification)
        {
            await _context.Notifications.AddAsync(Notification);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonHelper.SerializeWithReferenceHandling(Notification);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return Notification;
        }

        public async Task<List<Notification>> GetUserNotifications(string userCode)
        {
            var result = await _context.Notifications
                .Where(x => x.UserCode == userCode)
                .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<bool> MarkAsRead(long notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            // Broadcast the task to all connected clients
            await _webSocketHandler.BroadcastAsync("Notification has been viewed");

            return true;
        }
    }
}
