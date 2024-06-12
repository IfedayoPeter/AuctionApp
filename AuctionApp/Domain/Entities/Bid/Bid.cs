using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;

namespace AuctionApp.Domain.Entities.Bid
{
    public class Bid : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BidId { get; set; }
        public string BidCode {get; set;} 
        public string AuctionCode {get; set;}   
        public string UserCode {get; set;}  
        public decimal Amount {get; set;}  
        public decimal maxAmount {get; set;} 
        public bool autoIncrease {get; set;} 
    }
}