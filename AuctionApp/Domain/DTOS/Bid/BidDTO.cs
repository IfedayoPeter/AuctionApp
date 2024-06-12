namespace AuctionApp.Domain.DTOs.Bid
{
    public class BidDTO
    {
        public long BidId { get; set; }
        public string BidCode {get; set;} // Unique identifier for the bid
        public string AuctionCode {get; set;}   // Identifier for the associated auction
        public string UserCode {get; set;}  // Identifier for the user placing the bid
        public decimal Amount {get; set;}  // Amount of the bid
        public decimal maxAmount {get; set;} 
        public bool autoIncrease {get; set;} 
    }
}