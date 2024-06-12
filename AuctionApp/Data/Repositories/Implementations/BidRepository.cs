using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class BidRepository : IBidRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;
        public BidRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
        }

        public async Task<Bid> SubmitBid(Bid Bid)
        {
            await _context.Bids.AddAsync(Bid);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(Bid, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return Bid;
        } 

        public async Task<List<Bid>> GetAllBids()
        {
            var result = await _context.Bids.ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
        public async Task<Bid> GetHighestBid(string AuctionCode)
        {
            var result = await  _context.Bids
            .Where(x => x.AuctionCode == AuctionCode)
            .OrderByDescending(x => x.Amount)
            .FirstOrDefaultAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
       
        public async Task<bool> UpdateBid(Bid Bid)
        {
            //var result = _context.Bids.Update(Bid);
            _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            await _webSocketHandler.BroadcastAsync("Bid details updated successfully");

            return true;

        }

        public async Task<Bid> GetBidByCode(string BidCode)
        {
            var result = await _context.Bids
            .Where(x => x.BidCode == BidCode)
            .FirstOrDefaultAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
    }
}
