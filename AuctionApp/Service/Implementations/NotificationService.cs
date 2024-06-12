using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Notification;
using AuctionApp.Domain.Enums;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;
using AuctionApp.Data;
using Newtonsoft.Json;

namespace AuctionApp.Service.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRoomRepository _bidRoomRepository;
        private readonly RabbitMQService _rabbitMqService;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;
        private readonly WebSocketHandler _webSocketHandler;

        public NotificationService(
            INotificationRepository notificationRepository,
            IAuctionRepository auctionRepository,
            IBidRoomRepository bidRoomRepository,
            RabbitMQService rabbitMqService,
            WebSocketHandler webSocketHandler,
            ILogger<NotificationService> logger,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _auctionRepository = auctionRepository;
            _bidRoomRepository = bidRoomRepository;
            _webSocketHandler = webSocketHandler;
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<Notification>> CreateNotification(string message, string roomCode)
        {
            Result<Notification> result = new(false);

            try
            {
                var room = await _bidRoomRepository.GetBidRoomByCode(roomCode);
                if (room != null)
                {
                    foreach (var participant in room.ActiveParticipants)
                    {
                        var notification = new Notification
                        {
                            UserCode = participant.UserCode,
                            Message = message,
                            IsRead = false
                        };
                        await _notificationRepository.CreateNotification(notification);
                    }
                }
                await _webSocketHandler.BroadcastAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating notification");
                result.SetError(ex.ToString(), "Error while creating notification");
            }

            return result;
        }

        public async Task<Result<Notification>> CreateBidNotification(string message, string auctionCode)
        {
            Result<Notification> result = new(false);

            try
            {
                var auction = await _auctionRepository.GetAuctionByCode(auctionCode);
                var room = await _bidRoomRepository.GetBidRoomByCode(auction.RoomCode);
                if (room != null)
                {
                    foreach (var participant in room.ActiveParticipants)
                    {
                        var notification = new Notification
                        {
                            UserCode = participant.UserCode,
                            Message = message,
                            IsRead = false
                        };
                        await _notificationRepository.CreateNotification(notification);
                    }
                }
                await _webSocketHandler.BroadcastAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating notification");
                result.SetError(ex.ToString(), "Error while creating notification");
            }

            return result;
        }


        public async Task<Result<List<Notification>>> GetUserNotifications(string userCode)
        {
            Result<List<Notification>> result = new(false);

            try
            {
                var response = await _notificationRepository.GetUserNotifications(userCode);

                if (response.Count == 0 )
                {
                    result.SetError("user has no notification", "Notification does not exist");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<List<Notification>>(response), "Notifications retrieved successfully");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Notification");
                result.SetError(e.ToString(), "Notifications does not exist");
            }

            return result;
        }


        public async Task<Result<bool>> MarkAsRead(long id)
        {
            Result<bool> result = new(false);

            try
            {
                    var response = await _notificationRepository.MarkAsRead(id);
                    if (response == false)
                    {
                        result.SetError("Error", $"user has no unread notfication");
                    }
                    else
                    {
                        result.SetSuccess(response, $"Notification marked as read.");

                    }
                    result.Content = response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Notification");
                result.SetError(ex.ToString(), "Error while Updating Notification");
            }
            return result;
        }
    }
}
