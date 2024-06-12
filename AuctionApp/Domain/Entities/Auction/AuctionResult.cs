using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;
using AuctionApp.Domain.Enums;

namespace AuctionApp.Domain.Entities.Auction
{
    public class AuctionResult : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string AuctionCode { get; set; }
        public string ItemName { get; set; }
        public string HighestBidderCode { get; set; }
        public decimal HighestBidAmount { get; set; }
        public AuctionStatus Status { get; set; }
        public DateTime EndTime { get; set; }
    }
}