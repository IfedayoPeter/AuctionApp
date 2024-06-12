using System.Reflection;
using AuctionApp.Domain.Common;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Entities.Notification;
using AuctionApp.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionApp.Data
{
    public class CoreDbContext : DbContext, ICoreDbContext
    {
        private readonly DbContextOptions<CoreDbContext> options;
        //private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTime;

        public CoreDbContext(
        DbContextOptions<CoreDbContext> options,
        IDateTimeProvider dateTime) : base(options)

        {
            this.options = options;
            _dateTime = dateTime;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionResult> AuctionResults { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidRoom> BidRooms { get; set; }
        public DbSet<ActiveParticipants> ActiveParticipants { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime != null ? _dateTime.Now : DateTime.Now;
                        entry.Entity.LastModifiedOn = _dateTime != null ? _dateTime.Now : DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime != null ? _dateTime.Now : DateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<User>()
                    .ToTable("accounts")
                    .HasKey(x => x.Id);
            builder.Entity<Auction>()
                    .ToTable("auctions")
                    .HasKey(x => x.AuctionCode);
            builder.Entity<AuctionResult>()
                    .ToTable("auction_results")
                    .HasKey(x => x.AuctionCode);
            builder.Entity<Bid>()
                    .ToTable("bids")
                    .HasKey(x => x.BidCode);
            builder.Entity<BidRoom>()
                    .ToTable("bid_rooms")
                    .HasKey(x => x.RoomCode);
            builder.Entity<ActiveParticipants>()
                   .ToTable("active_participants")
                   .HasKey(x => x.UserCode);
            builder.Entity<Notification>()
                   .ToTable("notifications")
                   .HasKey(x => x.Id);

            builder.Entity<ActiveParticipants>()
                .HasOne(ap => ap.BidRoom)
                .WithMany(br => br.ActiveParticipants)
                .HasForeignKey(ap => ap.RoomCode)
                .HasPrincipalKey(br => br.RoomCode);

            base.OnModelCreating(builder);
        }
    }
}
