using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuctionApp.Domain.Common;
using AuctionApp.Domain.Enums;

namespace AuctionApp.Domain.Entities.User
{
    public class User : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserCode { get; set; }

        [Required]
        public string UserName { get; set; }

        public UserCategoryEnum UserCategory { get; set; }

        public string UserCategoryName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
