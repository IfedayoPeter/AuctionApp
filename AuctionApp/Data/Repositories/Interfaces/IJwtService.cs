using AuctionApp.Domain.Entities.User;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IJwtService
    {
         Task<string> GenerateJwtToken(Login login);
    }
}