using System.Text.Json;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class BidRoomRepository : IBidRoomRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;

        public BidRoomRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
        }

        public async Task<BidRoom> CreateBidRoom(BidRoom BidRoom)
        {
            await _context.BidRooms.AddAsync(BidRoom);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonHelper.SerializeWithReferenceHandling(BidRoom);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return BidRoom;
        }

        public async Task<List<BidRoom>> GetAllBidRooms()
        {
            var result = await _context.BidRooms
                .Include(x => x.ActiveParticipants)
                .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonHelper.SerializeWithReferenceHandling(result);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<List<BidRoom>> GetAllActiveBidRooms()
        {
            var result = await _context.BidRooms
            .Where(x => x.IsActive == true)
            .Include(x => x.ActiveParticipants)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonHelper.SerializeWithReferenceHandling(result);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
        public async Task<BidRoom> GetBidRoomByCode(string RoomCode)
        {
            var result = await _context.BidRooms
            .Where(x => x.RoomCode == RoomCode)
            .Include(x => x.ActiveParticipants)
            .FirstOrDefaultAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonHelper.SerializeWithReferenceHandling(result);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<bool> UpdateBidRoom(BidRoom BidRoom)
        {
            //var result = _context.BidRooms.Update(BidRoom);
            _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            await _webSocketHandler.BroadcastAsync("BidRoom details updated successfully");

            return true;

        }
    }
}
