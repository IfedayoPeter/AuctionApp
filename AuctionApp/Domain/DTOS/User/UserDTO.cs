using System.ComponentModel.DataAnnotations;
using AuctionApp.Domain.Enums;

namespace AuctionApp.Domain.DataTransferObject.User
{
    public partial class UserDTO
    {
        public long Id { get; set; }
        public string UserCode { get; set; }

        public string UserName { get; set; }

        public UserCategoryEnum UserCategory { get; set; }

        public string UserCategoryName { get { return this.UserCategory.ToString(); } }

        public string Password { get; set; }
    }
}
