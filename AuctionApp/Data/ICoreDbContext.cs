using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Entities.Notification;
using AuctionApp.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data
{
    public partial interface ICoreDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Auction> Auctions { get; set; }
        DbSet<AuctionResult> AuctionResults { get; set; }
        DbSet<Bid> Bids { get; set; }
        DbSet<BidRoom> BidRooms { get; set; }
        DbSet<ActiveParticipants> ActiveParticipants { get; set; }
        DbSet<Notification> Notifications { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
