using AuctionApp.Domain.Entities.Bid;

namespace AuctionApp.Domain.DTOs.Bid
{
    public class CreateBidRoomDTO
    {
        public long Id { get; set; }
        public string RoomCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? AuctionStartTime { get; set; }
        public DateTime? AuctionEndTime { get; set; }
        public string? CurrentAuctionCode { get; set; }
    }
}