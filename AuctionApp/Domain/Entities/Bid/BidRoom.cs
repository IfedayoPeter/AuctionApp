using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;

namespace AuctionApp.Domain.Entities.Bid
{
    public class BidRoom : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string RoomCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string? CurrentAuctionCode { get; set; }
        public List<ActiveParticipants>? ActiveParticipants { get; set; } = new List<ActiveParticipants>();
    }
}