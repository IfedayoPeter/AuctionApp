using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface ILoginService
    {
         Task<Result<string>> UserLogin(LoginDTO loginDTO);
    }
}