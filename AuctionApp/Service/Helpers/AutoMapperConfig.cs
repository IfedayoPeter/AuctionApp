using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Domain.Entities.Auction;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Domain.Entities.User;
using AutoMapper;

namespace AuctionApp.Service.Helpers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>()
                .ReverseMap();
            CreateMap<User, GetUserDTO>()
                .ReverseMap();
            CreateMap<UserRole, UserRoleDTO>()
                .ReverseMap();
            CreateMap<Bid, BidDTO>()
                .ReverseMap();
            CreateMap<ActiveParticipants, ActiveParticipantsDTO>()
                .ReverseMap();
            CreateMap<BidRoom, BidRoomDTO>()
                .ReverseMap(); 
            CreateMap<BidRoom, CreateBidRoomDTO>()
                .ReverseMap(); 
            CreateMap<Auction, AuctionDTO>()
                .ReverseMap();
            CreateMap<Auction, AuctionResultDTO>()
                .ReverseMap();
            CreateMap<AuctionResult, AuctionResultDTO>()
                .ReverseMap();
            CreateMap<Bid, UpdateBidDTO>()
                .ReverseMap();
            CreateMap<Auction, UpdateAuctionDTO>()
                .ReverseMap();
            CreateMap<Bid, Auction>()
                .ForMember(dest => dest.HighestBidAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.HighestBidderCode, opt => opt.MapFrom(src => src.UserCode))
                .ReverseMap();
            CreateMap<BidDTO, AuctionDTO>()
                .ForMember(dest => dest.HighestBidAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.HighestBidderCode, opt => opt.MapFrom(src => src.UserCode))
                .ReverseMap();
            CreateMap<Login, LoginDTO>()
                 .ReverseMap();

        }
    }
}
