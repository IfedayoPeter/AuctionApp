using AuctionApp.Domain.Entities.Bid;

namespace AuctionApp.Data.Repositories.Interfaces
{
    public interface IActiveParticipantsRepository
    {
        Task<ActiveParticipants> CreateActiveParticipants(ActiveParticipants ActiveParticipants);
        Task<ActiveParticipants> GetActiveParticipantsByCode(string UserCode);
        Task<List<ActiveParticipants>> GetAllActiveParticipants(string RoomCode);
    }
}
