using AuctionApp.Data;
using AuctionApp.Service.Implementations;
using AuctionApp.Service.Interfaces;

namespace AuctionApp.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<RabbitMQService>();
            services.AddHostedService<RabbitMQBackgroundService>();
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<IActiveParticipantsService, ActiveParticipantsService>();
            services.AddScoped<IBidRoomService, BidRoomService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<WebSocketHandler>();
            services.AddAutoMapper(typeof(DependencyInjection));

            return services;
        }
    }
}
