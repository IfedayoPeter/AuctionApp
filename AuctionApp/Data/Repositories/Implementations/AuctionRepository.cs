using System.Text.Json;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Enums;
using AuctionApp.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly WebSocketHandler _webSocketHandler;
        private readonly ICoreDbContext _context;

        public AuctionRepository(
            ICoreDbContext Context,
            WebSocketHandler webSocketHandler)
        {
            _context = Context;
            _webSocketHandler = webSocketHandler;
        }

        public async Task<Auction> CreateAuction(Auction Auction)
        {
            await _context.Auctions.AddAsync(Auction);
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(Auction, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return Auction;
        }

        public async Task<bool> UpdateAuction(Auction Auction)
        {
            await _context.SaveChangesAsync();
            // Broadcast the task to all connected clients
            await _webSocketHandler.BroadcastAsync("Auction details updated successfully");

            return true;
        }

        public async Task<List<Auction>> GetActiveAuctions()
        {
            var result = await _context.Auctions
            .Where(x => x.Status == AuctionStatus.Live)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }


        public async Task<List<Auction>> GetAllAuctions()
        {
            var result = await _context.Auctions.ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }

        public async Task<Auction> GetAuctionByCode(string AuctionCode)
        {
            var result = await _context.Auctions
            .Where(x => x.AuctionCode == AuctionCode)
            .FirstOrDefaultAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
  
        public async Task<List<Auction>> GetAuctionResult()
        {
            var result = await _context.Auctions
            .Where(x => x.Status == AuctionStatus.Closed)
            .OrderByDescending(x => x.HighestBidAmount)
            .ToListAsync();
            // Broadcast the task to all connected clients
            var taskJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            await _webSocketHandler.BroadcastAsync(taskJson);
            return result;
        }
    }
}
