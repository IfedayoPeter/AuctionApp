using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;

namespace AuctionApp.Domain.Entities.Bid
{
    public class ActiveParticipantsDTO : AuditableEntity
    {
        public long Id { get; set; }
        public string UserCode { get; set; }
        public string RoomCode { get; set; }
    }
}