using AuctionApp.Domain.Enums;

namespace AuctionApp.Domain.DTOs.Auction
{
    public class AuctionDTO
    {
        public long Id { get; set; }
        public string AuctionCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string SellerName { get; set; }
        public string RoomCode { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal ReservedPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } 
        public string HighestBidderCode { get; set; } 
        public decimal HighestBidAmount { get; set; } 
        public AuctionStatus Status { get; set; }
    }
}