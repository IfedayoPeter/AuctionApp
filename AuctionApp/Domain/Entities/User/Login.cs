using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Domain.Entities.User
{
    public partial class Login 
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
