using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class UserLoginRepository : ILoginRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;
        public UserLoginRepository(ICoreDbContext context, WebSocketHandler webSocketHandler, IPasswordHasher<User> passwordHasher,
        IJwtService jwtService)
        {
            _jwtService = jwtService;
            _webSocketHandler = webSocketHandler;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> UserLogin(Login login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == login.UserName);

            if (user != null && _passwordHasher.VerifyHashedPassword
            (user, user.Password, login.Password) == PasswordVerificationResult.Success)
            {
                // Broadcast the task to all connected clients
                await _webSocketHandler.BroadcastAsync("Login successful");

                return await _jwtService.GenerateJwtToken(login);
            }
            else
            {
                // Broadcast the task to all connected clients
                await _webSocketHandler.BroadcastAsync("Invalid login credentials");
                return "Invalid login credentials";
            }
        }

    }
}