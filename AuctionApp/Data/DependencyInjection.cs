using AuctionApp.Data.Repositories.Implementations;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Common;
using AuctionApp.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? "");
            });

            /*services.AddControllers(options =>
            {
                options.Filters.Add<CapitalizeStringPropertiesAttribute>();
            });
*/
            services.AddHttpContextAccessor();

            services.AddScoped<ICoreDbContext, CoreDbContext>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IBidRoomRepository, BidRoomRepository>();
            services.AddScoped<IActiveParticipantsRepository, ActiveParticipantsRepository>();
            services.AddScoped<ILoginRepository, UserLoginRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<WebSocketHandler>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            return services;
        }

    }
}