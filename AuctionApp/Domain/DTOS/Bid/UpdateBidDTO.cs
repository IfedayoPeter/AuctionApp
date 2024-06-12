namespace AuctionApp.Domain.DTOs.Bid
{
    public class UpdateBidDTO
    {
        public string BidCode {get; set;} 
        public decimal Amount {get; set;} 
        public decimal maxAmount {get; set;} 
        public bool autoIncrease {get; set;} 
    }
}