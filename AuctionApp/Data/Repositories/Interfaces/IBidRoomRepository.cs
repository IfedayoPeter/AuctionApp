using AuctionApp.Domain.Entities.Bid;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IBidRoomRepository
    {
        Task<BidRoom> CreateBidRoom(BidRoom BidRoom); 
        Task<List<BidRoom>> GetAllBidRooms();
        Task<List<BidRoom>> GetAllActiveBidRooms();
        Task<BidRoom> GetBidRoomByCode(string RoomCode);
        Task<bool> UpdateBidRoom(BidRoom BidRoom);
    }
}
