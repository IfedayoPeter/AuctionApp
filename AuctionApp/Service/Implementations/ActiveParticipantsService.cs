using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;

namespace AuctionApp.Service.Implementations
{
    public class ActiveParticipantsService : IActiveParticipantsService
    {
        private readonly IActiveParticipantsRepository _activeParticipantsRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ILogger<ActiveParticipantsService> _logger;
        private readonly IMapper _mapper;

        public ActiveParticipantsService(
            IActiveParticipantsRepository activeParticipantsRepository,
            IBidRepository bidRepository,
            ILogger<ActiveParticipantsService> logger,
            IMapper mapper)
        {
            _activeParticipantsRepository = activeParticipantsRepository;
            _bidRepository = bidRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<ActiveParticipantsDTO>> CreateActiveParticipants(ActiveParticipantsDTO activeParticipantsDTO)
        {
            Result<ActiveParticipantsDTO> result = new(false);

            try
            {
                var activeParticipants = _mapper.Map<ActiveParticipants>(activeParticipantsDTO);

                {
                    var response = await _activeParticipantsRepository.CreateActiveParticipants(activeParticipants);
                    result.SetSuccess(_mapper.Map<ActiveParticipantsDTO>(response), "ActiveParticipants submitted Successfully, Save your usercode to update activeParticipants");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while submitting activeParticipants");
                result.SetError(ex.ToString(), "Error while submitting activeParticipants");
            }
            return result;
        }
    }
}
