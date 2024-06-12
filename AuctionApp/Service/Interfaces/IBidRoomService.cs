using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface IBidRoomService
    {
        Task<Result<CreateBidRoomDTO>> CreateBidRoom(CreateBidRoomDTO BidRoomDTO);
        Task<Result<bool>> EnterBidRoom(string RoomCode, string UserCode);
        Task<Result<bool>> ExitBidRoom(string RoomCode, string UserCode);
        Task<Result<List<BidRoomDTO>>> GetAllBidRooms();
        Task<Result<List<BidRoomDTO>>> GetAllActiveBidRooms();
        Task<Result<BidRoomDTO>> GetBidRoomByCode(string RoomCode);
        Task<Result<List<ActiveParticipantsDTO>>> GetAllActiveParticipants(string RoomCode);
        Task<Result<bool>> UpdateBidRoom(string RoomCode, BidRoomDTO BidRoomDTO);
    }
}
