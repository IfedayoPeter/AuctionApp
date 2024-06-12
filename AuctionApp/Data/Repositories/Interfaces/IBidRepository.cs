using AuctionApp.Domain.Entities.Bid;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task<Bid> SubmitBid(Bid Bid); 
        Task<List<Bid>> GetAllBids();
        Task<Bid> GetHighestBid(string AuctionCode);
        Task<Bid> GetBidByCode(string BidCode);
        Task<bool> UpdateBid(Bid Bid);
    }
}
