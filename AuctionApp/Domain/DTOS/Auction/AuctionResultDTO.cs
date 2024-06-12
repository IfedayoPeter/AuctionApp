using AuctionApp.Domain.Enums;

namespace AuctionApp.Domain.DTOs.Auction
{
    public class AuctionResultDTO
    {
        public long Id { get; set; }
        public string AuctionCode { get; set; }
        public string ItemName { get; set; }
        public string HighestBidderCode { get; set; }
        public decimal HighestBidAmount { get; set; }
        public AuctionStatus Status { get; set; }
        public DateTime EndTime { get; set; }
    }
}