using AuctionApp.Domain.Common;

namespace AuctionApp.Domain.DataTransferObject.User
{
    public partial class LoginDTO 
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
