using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;

namespace AuctionApp.Domain.Entities.Notification
{
    public class Notification : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserCode { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }

}
