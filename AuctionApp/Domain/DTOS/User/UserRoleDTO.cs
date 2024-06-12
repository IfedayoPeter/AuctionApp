using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Domain.DataTransferObject.User
{
    public class UserRoleDTO
    {
        [Key]
        [Required]
        public int RoleId { get; set; }

        public string Name { get; set; }
    }
}
