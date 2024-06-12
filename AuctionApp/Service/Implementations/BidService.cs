using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Enums;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;

namespace AuctionApp.Service.Implementations
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly INotificationService _notificationService;
        private readonly RabbitMQService _rabbitMqService;
        private readonly ILogger<BidService> _logger;
        private readonly IMapper _mapper;

        public BidService(
            IBidRepository bidRepository,
            IAuctionRepository auctionRepository,
            INotificationService notificationService,
            RabbitMQService rabbitMqService,
            ILogger<BidService> logger,
            IMapper mapper)
        {
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
            _notificationService = notificationService;
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<BidDTO>> SubmitBid(BidDTO bidDTO)
        {
            Result<BidDTO> result = new(false);

            try
            {
                bidDTO.BidCode = new RandomGenerator().GenerateRandomCode(5);

                var auction = await _auctionRepository.GetAuctionByCode(bidDTO.AuctionCode);

                if (auction == null)
                {
                    result.SetError("Auction does not exist", "check your auction code and try again");
                }
                else
                {
                    var bid = _mapper.Map<Bid>(bidDTO);
                    var existingBid = await _bidRepository.GetAllBids();
                    if (existingBid.Any(existingBid => existingBid.BidCode == bid.BidCode))
                    {
                        result.SetError("You already have an existing bid", "Update you existing bid to counter offers");
                    }
                    
                    else
                    {
                        if (DateTime.UtcNow < auction.StartTime) { result.SetError("Auction has not started", $"Check back at{auction.StartTime}"); }

                        else
                        {
                            var response = await _bidRepository.SubmitBid(bid);
                            result.SetSuccess(_mapper.Map<BidDTO>(response), "Bid submitted Successfully, Save your bidcode to update bid");

                            string message = $"New bid submitted: {response.Amount}";
                            await _notificationService.CreateBidNotification(message, response.AuctionCode);//sends notification to all participant

                            var publish = _mapper.Map<BidDTO>(response);
                            _rabbitMqService.PublishBidSubmitted(publish); // publish bid submitted

                            if (auction.Status == AuctionStatus.NotStarted)
                            {
                                auction.Status = AuctionStatus.Live;

                                string notify = $"Auction is live: {response.AuctionCode}";
                                await _notificationService.CreateBidNotification(message, response.AuctionCode);//sends notification to all participant

                                var publishAuction = _mapper.Map<AuctionDTO>(auction);
                                _rabbitMqService.PublishAuctionStarted(publishAuction);//Publish auction started
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while submitting bid");
                result.SetError(ex.ToString(), "Error while submitting bid");
            }
            return result;
        }

        public async Task<Result<BidDTO>> GetBidByCode(string bidCode)
        {
            Result<BidDTO> result = new(false);

            try
            {
                var response = await _bidRepository.GetBidByCode(bidCode);

                if (response == null)
                {
                    result.SetError("Error retrieving bid", "Bid does not exist");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<BidDTO>(response), "Bid retrived successfully");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Bid");
                result.SetError(e.ToString(), "Bids does not exist");
            }

            return result;
        }


        public async Task<Result<List<BidDTO>>> GetAllBids()
        {
            Result<List<BidDTO>> result = new(false);

            try
            {
                var response = await _bidRepository.GetAllBids();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving bid", "Bid does not exist");
                }
                else
                {
                    var bid = _mapper.Map<List<BidDTO>>(response);
                    result.SetSuccess(bid, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Bid ");
                result.SetError(e.ToString(), "Bids does not exist");
            }

            return result;
        }

        public async Task<Result<BidDTO>> GetHighestBid(string AuctionCode)
        {
            Result<BidDTO> result = new(false);

            try
            {
                var response = await _bidRepository.GetHighestBid(AuctionCode);

                var publish = _mapper.Map<BidDTO>(response);

                string message = $"New Highest bid: {response.Amount}";
                await _notificationService.CreateBidNotification(message, response.AuctionCode);//sends notification to all participant

                _rabbitMqService.PublishCurrentHighestBid(publish);// publish current highest bid

                if (response == null)
                {
                    result.SetError("Error retrieving bid", "Bid does not exist");
                }
                else
                {
                    // update highestbidder and highest bidder amount in auction
                    var auction = await _auctionRepository.GetAuctionByCode(response.AuctionCode);
                    if (auction != null)
                    {
                        // Map changes to the existing auction entity
                        _mapper.Map(response, auction);

                        // Update the auction in the repository
                        await _auctionRepository.UpdateAuction(auction);

                        var bid = _mapper.Map<BidDTO>(response);
                        result.SetSuccess(bid, "Retrieved Successfully");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving Bid ");
                result.SetError(e.ToString(), "Bids does not exist");
            }

            return result;
        }

        public async Task<Result<bool>> UpdateBid(string BidCode, UpdateBidDTO BidDTO)
        {
            Result<bool> result = new(false);

            try
            {
                var existingBid = await _bidRepository.GetBidByCode(BidCode);
                if (existingBid == null)
                {
                    result.SetError("Bid not updated", $"Bid with Code {BidCode} does not exist");
                }
                else
                {
                    _mapper.Map(BidDTO, existingBid);
                    var response = await _bidRepository.UpdateBid(existingBid);
                    if (!response)
                    {
                        result.SetError("Bid not updated", $"Bid with Code {BidCode} not updated");
                    }
                    else
                    {
                        result.SetSuccess(response, $"Bid with Code {BidCode} updated Successfully.");

                        string message = $"A bid has been updated: {existingBid.Amount}";
                        await _notificationService.CreateBidNotification(message, existingBid.AuctionCode);//sends notification to all participant

                        var publish = _mapper.Map<BidDTO>(existingBid);
                        _rabbitMqService.PublishBidUpdated(publish); // publish updated bid
                    }
                    result.Content = response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while Updating Bid");
                result.SetError(ex.ToString(), "Error while Updating Bid");
            }
            return result;
        }
    }
}
