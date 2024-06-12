using AuctionApp.Domain.Entities.User;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface ILoginRepository
    {
         Task<string> UserLogin(Login login);
    }
}