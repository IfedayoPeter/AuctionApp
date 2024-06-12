using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface IAuctionService
    {
        Task<Result<AuctionDTO>> CreateAuction(AuctionDTO AuctionDTO);
        Task<Result<AuctionDTO>> GetAuctionByCode(string AuctionCode);
        Task<Result<List<AuctionDTO>>> GetAllAuctions();
        Task<Result<List<AuctionDTO>>> GetActiveAuctions();
        Task<Result<List<AuctionResultDTO>>> GetAuctionResult();
        Task<Result<List<AuctionDTO>>> EndAuction();
        Task<Result<List<AuctionDTO>>> CheckAndStartAuction();
        Task<Result<bool>> UpdateAuction(string AuctionCode, UpdateAuctionDTO AuctionDTO);
    }
}
