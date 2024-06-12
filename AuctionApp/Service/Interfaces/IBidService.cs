using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface IBidService
    {
        Task<Result<BidDTO>> SubmitBid(BidDTO BidDTO);
        Task<Result<List<BidDTO>>> GetAllBids();
        Task<Result<BidDTO>> GetHighestBid(string AuctionCode);
        Task<Result<BidDTO>> GetBidByCode(string BidCode);
        Task<Result<bool>> UpdateBid(string BidCode, UpdateBidDTO BidDTO);
    }
}
