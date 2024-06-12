using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;
using Azure;

namespace AuctionApp.Service.Implementations
{
    public class BidRoomService : IBidRoomService
    {
        private readonly IBidRoomRepository _bidRoomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IActiveParticipantsRepository _activeParticipantRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<BidRoomService> _logger;
        private readonly IMapper _mapper;
        private readonly RabbitMQService _rabbitMqService;

        public BidRoomService(
            IBidRoomRepository bidRoomRepository,
            IActiveParticipantsRepository activeParticipantRepository,
            IAuctionRepository auctionRepository,
            INotificationService notificationService,
            IUserRepository userRepository,
            RabbitMQService rabbitMqService,
            ILogger<BidRoomService> logger,
            IMapper mapper)
        {
            _bidRoomRepository = bidRoomRepository;
            _activeParticipantRepository = activeParticipantRepository;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<CreateBidRoomDTO>> CreateBidRoom(CreateBidRoomDTO bidRoomDTODTO)
        {
            Result<CreateBidRoomDTO> result = new(false);

            try
            {
                bidRoomDTODTO.RoomCode = new RandomGenerator().GenerateRandomCode(5);
                bidRoomDTODTO.IsActive = false;

                var room = _mapper.Map<BidRoom>(bidRoomDTODTO);
                var response = await _bidRoomRepository.CreateBidRoom(room);
                if (response == null)
                {
                    result.SetError("Room not created", "Try a different roomName");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<CreateBidRoomDTO>(response), "Room Created Successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Account");
                result.SetError(ex.ToString(), "Error while creating Account");
            }
            return result;
        }

        public async Task<Result<List<BidRoomDTO>>> GetAllBidRooms()
        {
            Result<List<BidRoomDTO>> result = new(false);

            try
            {
                var response = await _bidRoomRepository.GetAllBidRooms();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving bidRoom", "Room does not exist");
                }
                else
                {
                    var bidRoom = _mapper.Map<List<BidRoomDTO>>(response);
                    result.SetSuccess(bidRoom, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving BidRoom ");
                result.SetError(e.ToString(), "BidGetAllBidRooms does not exist");
            }

            return result;
        }

        public async Task<Result<List<BidRoomDTO>>> GetAllActiveBidRooms()
        {
            Result<List<BidRoomDTO>> result = new(false);

            try
            {
                var response = await _bidRoomRepository.GetAllActiveBidRooms();

                if (response.Count == 0)
                {
                    result.SetError("Error!", "No Active Bid room");
                }
                else
                {
                    var bidRoom = _mapper.Map<List<BidRoomDTO>>(response);
                    result.SetSuccess(bidRoom, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "No active bid room");
                result.SetError(e.ToString(), "No active bid room");
            }

            return result;
        }

        public async Task<Result<List<ActiveParticipantsDTO>>> GetAllActiveParticipants(string RoomCode)
        {
            Result<List<ActiveParticipantsDTO>> result = new(false);

            try
            {
                var response = await _activeParticipantRepository.GetAllActiveParticipants(RoomCode);

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving bidRoom", "Room does not exist");
                }
                else
                {
                    var bidRoom = _mapper.Map<List<ActiveParticipantsDTO>>(response);
                    result.SetSuccess(bidRoom, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving BidRoom ");
                result.SetError(e.ToString(), "Error retrieving bid room");
            }

            return result;
        }

        public async Task<Result<BidRoomDTO>> GetBidRoomByCode(string RoomCode)
        {
            Result<BidRoomDTO> result = new(false);

            try
            {
                var response = await _bidRoomRepository.GetBidRoomByCode(RoomCode);

                if (response == null)
                {
                    result.SetError("Error retrieving bidRoom", "Room does not exist");
                }
                else
                {
                    var bidRoom = _mapper.Map<BidRoomDTO>(response);
                    result.SetSuccess(bidRoom, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving BidRoom ");
                result.SetError(e.ToString(), "BidGetAllBidRooms does not exist");
            }

            return result;
        }

        public async Task<Result<bool>> UpdateBidRoom(string RoomCode, BidRoomDTO BidRoomDTO)
        {
            Result<bool> result = new(false);

            try
            {
                var existingRoom = await _bidRoomRepository.GetBidRoomByCode(RoomCode);
                if (existingRoom == null)
                {
                    result.SetError("Room not updated", $"Room with Code {RoomCode} does not exist");
                }
                _mapper.Map(BidRoomDTO, existingRoom);
                var response = await _bidRoomRepository.UpdateBidRoom(existingRoom);
                if (!response)
                {
                    result.SetError("Room not updated", $"Room with Code {RoomCode} not updated");
                }
                else
                {
                    result.SetSuccess(response, $"Room with Code {RoomCode} updated Successfully.");
                }

                result.Content = response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Room");
                result.SetError(ex.ToString(), "Error while Updating Room");
            }
            return result;
        }

        public async Task<Result<bool>> EnterBidRoom(string RoomCode, string UserCode)
        {
            Result<bool> result = new Result<bool>(false);

            try
            {
                var room = await _bidRoomRepository.GetBidRoomByCode(RoomCode);
                if (room == null)
                {
                    result.SetError("Error", $"Room with Code {RoomCode} does not exist");
                }
                else
                {
                    var user = await _userRepository.GetUserByCode(UserCode);
                    if (user == null)
                        result.SetError("Error", $"User with Code {UserCode} does not exist");

                    else
                    {
                        if (room.ActiveParticipants.Count == 0)
                        {
                            new List<ActiveParticipants>();
                            var activeParticipantDto = new ActiveParticipantsDTO
                            {
                                UserCode = UserCode,
                                RoomCode = RoomCode
                            };

                            var activeParticipant = _mapper.Map<ActiveParticipants>(activeParticipantDto);

                            await _activeParticipantRepository.CreateActiveParticipants(activeParticipant);

                                room.ActiveParticipants.Add((ActiveParticipants)activeParticipant);

                                room.IsActive = true; //sets room to active when a participant enters the room

                                var auction = await _auctionRepository.GetAuctionByCode(room.CurrentAuctionCode);
                                if (auction == null){ result.SetError("Auction does not exit", $"Auvtion with Code {RoomCode} does not exist"); }
                                else
                                {
                                    string message = $"Auction is now live: {auction.AuctionCode}";
                                    await _notificationService.CreateNotification(message, auction.RoomCode);//sends notification to all participant

                                var publish = _mapper.Map<AuctionDTO>(auction);
                                    _rabbitMqService.PublishAuctionStarted(publish); //Publish auction has started
                                }

                            var response = await _bidRoomRepository.UpdateBidRoom(room);
                            string notify = $"A new participant has joined the room: {auction.RoomCode}";
                            await _notificationService.CreateNotification(notify, auction.RoomCode);//sends notification to all participant
                            if (!response)
                            {
                                result.SetError("Unable to enter room", $"Room with Code {RoomCode} not updated");
                            }
                        }

                        result.SetSuccess(true, "You have successfully entered bidding room");
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Room");
                result.SetError(ex.ToString(), "Error while Updating Room");
            }

            return result;
        }

        public async Task<Result<bool>> ExitBidRoom(string RoomCode, string UserCode)
        {
            Result<bool> result = new Result<bool>(false);

            try
            {
                var room = await _bidRoomRepository.GetBidRoomByCode(RoomCode);
                if (room == null)
                    result.SetError("Error", $"Room with Code {RoomCode} does not exist");
                else
                {
                    var user = await _userRepository.GetUserByCode(UserCode);
                    if (user == null)
                        result.SetError("Error", $"User with Code {UserCode} does not exist");
                }

                var participant = room.ActiveParticipants.FirstOrDefault(ap => ap.UserCode == UserCode);
                if (participant == null)
                     result.SetError("Error", $"User with Code {UserCode} is not in the room");
                else
                {
                    room.ActiveParticipants.Remove(participant);//removes user from room

                    string message = $"A participant has left the room: {participant.UserCode}";
                    await _notificationService.CreateNotification(message, participant.RoomCode);//sends notification to all participant

                    if (room.IsActive = room.ActiveParticipants.Count <= 0)
                    {
                        room.IsActive = false;  //sets room to inactive when room is empty
                    }
                }
                
                var response = await _bidRoomRepository.UpdateBidRoom(room);
                if (!response)
                    result.SetError("Unable to leave room", $"Room with Code {RoomCode} not updated");
                else
                {
                    result.SetSuccess(true, "You have successfully left bidding room");
                }
                
                result.Content = response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Room");
                result.SetError(ex.ToString(), "Error while Updating Room");
            }
            return result;
        }


    }
}

