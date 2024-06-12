using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;

namespace AuctionApp.Service.Interfaces
{
    public interface IActiveParticipantsService
    {
        Task<Result<ActiveParticipantsDTO>> CreateActiveParticipants(ActiveParticipantsDTO ActiveParticipantsDTO);
    }
}
