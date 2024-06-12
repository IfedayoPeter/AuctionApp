using System.Text.Json;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.User;
using AuctionApp.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler,
            IPasswordHasher<User> passwordHasher)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUser(User user)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(user, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return user;
        }

        public async Task<bool> DeleteUserAccount(string userCode)
        {

            var user = await _context.Users
                       .Where(x => x.UserCode == userCode)
                       .FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                // Broadcast the task to all connected clients
                await _webSocketHandler.BroadcastAsync("User deleted successfully");
                return true;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            var result = await _context.Users.ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
        public async Task<List<User>> GetAllBuyers()
        {
            var result = await _context.Users
            .Where(x => x.UserCategory == UserCategoryEnum.Buyer)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<List<User>> GetAllSellers()
        {
            var result = await _context.Users
            .Where(x => x.UserCategory == UserCategoryEnum.Seller)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<User> GetUserByCode(string userCode)
        {
            var result = await _context.Users
            .Where(x => x.UserCode == userCode)
            .FirstOrDefaultAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<List<User>> GetUserByName(string userName)
        {
            var result = await _context.Users
            .Where(x => x.UserName == userName)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<bool> UpdateUserAccount(User user)
        {
            //var result = _context.Users.Update(user);
            _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            await _webSocketHandler.BroadcastAsync("User details updated successfully");

            return true;

        }
    }
}
