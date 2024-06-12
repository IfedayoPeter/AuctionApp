using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDTO>> CreateAccount(UserDTO userDTO);        Task<Result<List<GetUserDTO>>> GetUsers();
        Task<Result<List<GetUserDTO>>> GetBuyers();
        Task<Result<List<GetUserDTO>>> GetSellers();
        Task<Result<GetUserDTO>> GetUserByCode(string userCode);
        Task<Result<List<GetUserDTO>>> GetUserByUserName(string userName);     
        Task<Result<bool>> DeleteUserAccount(string userCode);

    }
}
