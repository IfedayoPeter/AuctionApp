using AuctionApp.Domain.Entities.Auction;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> CreateAuction(Auction Auction);
        Task<Auction> GetAuctionByCode(string AuctionCode);
        Task<List<Auction>> GetAllAuctions();
        Task<List<Auction>> GetActiveAuctions();
        Task<List<Auction>> GetAuctionResult();
        Task<bool> UpdateAuction(Auction Auction);
    }
}
