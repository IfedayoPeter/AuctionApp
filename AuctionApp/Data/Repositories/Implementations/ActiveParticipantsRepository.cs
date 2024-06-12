using System.Text.Json;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Bid;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class ActiveParticipantsRepository : IActiveParticipantsRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;

        public ActiveParticipantsRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
        }

        public async Task<ActiveParticipants> CreateActiveParticipants(ActiveParticipants ActiveParticipants)
        {
            await _context.ActiveParticipants.AddAsync(ActiveParticipants);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            //var taskJson = JsonSerializer.Serialize(ActiveParticipants);
            //await _webSocketHandler.BroadcastAsync(taskJson);
            return ActiveParticipants;
        }

        public async Task<ActiveParticipants> GetActiveParticipantsByCode(string UserCode)
        {
            var result = await _context.ActiveParticipants
                .Where(x => x.UserCode == x.UserCode)
                .FirstOrDefaultAsync();

            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async  Task<List<ActiveParticipants>> GetAllActiveParticipants(string RoomCode)
        {
            var result = await _context.ActiveParticipants
                .Where(x => x.RoomCode == x.RoomCode)
                .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

    }
}
