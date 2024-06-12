using AuctionApp.Domain.Entities.User;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<bool> DeleteUserAccount(string userCode);
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetAllBuyers();
        Task<List<User>> GetAllSellers();
        Task<User> GetUserByCode(string userCode);
        Task<List<User>> GetUserByName(string userName);
        Task<bool> UpdateUserAccount(User user);
    }
}
