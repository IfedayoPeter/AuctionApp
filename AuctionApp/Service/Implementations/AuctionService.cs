using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Enums;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;

namespace AuctionApp.Service.Implementations
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly INotificationService _notificationService;
        private readonly RabbitMQService _rabbitMqService;
        private readonly ILogger<AuctionService> _logger;
        private readonly IMapper _mapper;

        public AuctionService(
            IAuctionRepository auctionRepository,
            IBidRepository bidRepository,
            INotificationService notificationService,
            RabbitMQService rabbitMqService,
            ILogger<AuctionService> logger,
            IMapper mapper)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _notificationService = notificationService;
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<AuctionDTO>> CreateAuction(AuctionDTO auctionDTO)
        {
            Result<AuctionDTO> result = new(false);

            try
            {
                auctionDTO.AuctionCode = new RandomGenerator().GenerateRandomCode(5);
                if (auctionDTO.StartTime == DateTime.UtcNow)
                {
                    auctionDTO.Status = AuctionStatus.Live;

                    string message = $"Auction is live: {auctionDTO.AuctionCode}";
                    await _notificationService.CreateNotification(message, auctionDTO.RoomCode);//sends notification to all participant
                   
                    _rabbitMqService.PublishAuctionStarted(auctionDTO); //publish auction started
                }
                else
                {
                    auctionDTO.Status = AuctionStatus.NotStarted;
                }
                var auction = _mapper.Map<Auction>(auctionDTO);

                {
                    var response = await _auctionRepository.CreateAuction(auction);
                    result.SetSuccess(_mapper.Map<AuctionDTO>(response), "Auction submitted Successfully, Save your auctioncode to update auction");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while submitting auction");
                result.SetError(ex.ToString(), "Error while submitting auction");
            }
            return result;
        }

        public async Task<Result<AuctionDTO>> GetAuctionByCode(string auctionCode)
        {
            Result<AuctionDTO> result = new(false);

            try
            {
                var response = await _auctionRepository.GetAuctionByCode(auctionCode);

                if (response == null)
                {
                    result.SetError("Error retrieving auction", "Auction does not exist");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<AuctionDTO>(response), "Auction retrived successfully");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Auction");
                result.SetError(e.ToString(), "Auctions does not exist");
            }

            return result;
        }

        public async Task<Result<List<AuctionDTO>>> GetAllAuctions()
        {
            Result<List<AuctionDTO>> result = new(false);

            try
            {
                var response = await _auctionRepository.GetAllAuctions();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving auction", "No Auction has been created");
                }
                else
                {
                    var auction = _mapper.Map<List<AuctionDTO>>(response);
                    result.SetSuccess(auction, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Auction ");
                result.SetError(e.ToString(), "No Auction has been created");
            }

            return result;
        }

        public async Task<Result<List<AuctionDTO>>> GetActiveAuctions()
        {
            Result<List<AuctionDTO>> result = new(false);

            try
            {
                var response = await _auctionRepository.GetActiveAuctions();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving auction", "No current live auction");
                }
                else
                {
                    var auction = _mapper.Map<List<AuctionDTO>>(response);
                    result.SetSuccess(auction, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Auction ");
                result.SetError(e.ToString(), "No current live auction");
            }

            return result;
        }

        public async Task<Result<List<AuctionResultDTO>>> GetAuctionResult()
        {
            Result<List<AuctionResultDTO>> result = new(false);

            try
            {
                var response = await _auctionRepository.GetAuctionResult();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving auction results", "Auction has not been concluded");
                }
                else
                {
                    var auction = _mapper.Map<List<AuctionResultDTO>>(response);
                    result.SetSuccess(auction, "Retrieved Successfully");

                    foreach (var auctionResult in auction)
                    {
                        string message = $"Auction Winner: {auctionResult.HighestBidderCode}";
                        await _notificationService.CreateBidNotification(message,auctionResult.AuctionCode );//sends notification to all participant

                        _rabbitMqService.PublishAuctionResult(auctionResult); //publish auction result
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Auction ");
                result.SetError(e.ToString(), "Auctions does not exist");
            }

            return result;
        }

        public async Task<Result<bool>> UpdateAuction(string AuctionCode, UpdateAuctionDTO UpdateAuctionDTO)
        {
            Result<bool> result = new(false);

            try
            {
                var existingAuction = await _auctionRepository.GetAuctionByCode(AuctionCode);
                if (existingAuction == null)
                {
                    result.SetError("Auction not updated", $"Auction with Code {AuctionCode} does not exist");
                }
                _mapper.Map(UpdateAuctionDTO, existingAuction);
                var response = await _auctionRepository.UpdateAuction(existingAuction);
                if (!response)
                {
                    result.SetError("Auction not updated", $"Auction with Id {AuctionCode} not updated");
                }
                else
                {
                    result.SetSuccess(response, $"Auction with Id {AuctionCode} updated Successfully.");
                }

                result.Content = response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Auction");
                result.SetError(ex.ToString(), "Error while Updating Auction");
            }
            return result;
        }

        public async Task<Result<List<AuctionDTO>>> EndAuction()
        {
            Result<List<AuctionDTO>> result = new(false);

            try
            {
                var auctions = await _auctionRepository.GetAllAuctions();

                if (auctions.Any(x => x.Status == AuctionStatus.Live))
                {
                    var updatedAuctions = new List<AuctionDTO>();

                    foreach (var auction in auctions)
                    {
                        var highestBid = await _bidRepository.GetHighestBid(auction.AuctionCode);

                        if (highestBid != null && highestBid.Amount >= auction.ReservedPrice || DateTime.UtcNow >= auction.EndTime)
                        {
                            auction.Status = AuctionStatus.Closed;

                            string message = $"Auction Has Ended: {auction.AuctionCode}";
                            await _notificationService.CreateNotification(message, auction.RoomCode);//sends notification to all participant

                            var publish =  _mapper.Map<AuctionDTO>(auction);
                            _rabbitMqService.PublishEndAuction(publish);// publish auction has ended

                            await _auctionRepository.UpdateAuction(auction);
                            updatedAuctions.Add(_mapper.Map<AuctionDTO>(auction));

                            var auctionResult = new AuctionResultDTO()
                            {
                                AuctionCode = auction.AuctionCode,
                                ItemName = auction.ItemName,
                                HighestBidderCode = auction.HighestBidderCode,
                                HighestBidAmount = auction.HighestBidAmount,
                                Status = auction.Status,
                                EndTime = auction.EndTime
                            };
                        }
                    }

                    if (updatedAuctions.Count == 0)
                    {
                        result.SetError("No auctions closed", "No auctions met the reserve price");
                    }

                    result.SetSuccess(updatedAuctions, "Auctions closed successfully");
                }
                else
                {
                    if (auctions.Any(x => x.Status == AuctionStatus.Closed) || auctions.Count == 0)
                    {
                        result.SetError("Error retrieving auction", "Auction not found or is already ended");
                    }
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Auction ");
                result.SetError(e.ToString(), "Auctions does not exist");
            }

            return result;
        }

        public async Task<Result<List<AuctionDTO>>> CheckAndStartAuction()
        {
            Result<List<AuctionDTO>> result = new(false);

            try
            {
                var auctions = await _auctionRepository.GetAllAuctions();

                if (auctions.Any(x => x.Status == AuctionStatus.NotStarted)
                    || auctions.Any(x => x.Status == AuctionStatus.Closed)  || auctions.Count == 0)
                {
                    result.SetError("No AAuction was started", "No auction has been created or all auctions has either started or ended");
                }
                else
                {
                    var updatedAuctions = new List<AuctionDTO>();

                    foreach (var auction in auctions)
                    {
                        if (DateTime.UtcNow >= auction.StartTime)
                        {
                            auction.Status = AuctionStatus.Live;

                            string message = $"Auction is now live: {auction.AuctionCode}";
                            await _notificationService.CreateNotification(message, auction.RoomCode);//sends notification to all participant

                            var publish = _mapper.Map<AuctionDTO>(auction);
                            _rabbitMqService.PublishAuctionStarted(publish); //publish Auctionstarted
                            await _auctionRepository.UpdateAuction(auction);
                            updatedAuctions.Add(_mapper.Map<AuctionDTO>(auction));
                        }
                    }

                    if (updatedAuctions.Count == 0)
                    {
                        result.SetError("No auctions was started", "Not yet auction start time");
                    }

                    result.SetSuccess(updatedAuctions, "Auctions started successfully");
                }
               
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error starting Auction ");
                result.SetError(e.ToString(), "Error starting auction");
            }

            return result;
        }

    }
}
