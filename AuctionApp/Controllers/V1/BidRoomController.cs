using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Domain.Entities.Bid;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    [Authorize]
    [Route("api/v{version}/bidRoom/[controller]")]
    //[Route("api/bidRoom/[controller]")]
    [ApiController]
    public class BidRoomController : BaseController
    {
        private readonly IBidRoomService _bidRoomService;
        private readonly IActiveParticipantsService _activeParticipants;

        public BidRoomController(IBidRoomService bidRoomService, IActiveParticipantsService activeParticipants)
        {
            _bidRoomService = bidRoomService;
            _activeParticipants = activeParticipants;
            _activeParticipants = activeParticipants;

        }

        [HttpPost("create_participant")]
        public async Task<ActionResult> CreateActiveParticipants(ActiveParticipantsDTO participantsDTO)
        {
            var result = new Result<ActiveParticipantsDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _activeParticipants.CreateActiveParticipants(participantsDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }
        
        [HttpPost("create_bidRoom")]
        public async Task<ActionResult> CreateBidRoom(CreateBidRoomDTO bidRoomDTO)
        {
            var result = new Result<CreateBidRoomDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.CreateBidRoom(bidRoomDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_bidRoom_by_code")]
        public async Task<ActionResult> GetBidRoomByCode(string bidRoomCode)
        {
            var result = new Result<BidRoomDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.GetBidRoomByCode(bidRoomCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_bidRooms")]
        public async Task<ActionResult> GetAllBidRooms()
        {
            var result = new Result<List<BidRoomDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.GetAllBidRooms();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_active_bidRooms")]
        public async Task<ActionResult> GetActiveBidRooms()
        {
            var result = new Result<List<BidRoomDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.GetAllActiveBidRooms();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_active_participants")]
        public async Task<ActionResult> GetActiveParticipants(string RoomCode)
        {
            var result = new Result<List<ActiveParticipantsDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.GetAllActiveParticipants(RoomCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }


        [HttpPost]
        [Route("exit_bidRoom")]
        public async Task<ActionResult> ExitBidRoom(string RoomCode, string UserCode)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.ExitBidRoom(RoomCode, UserCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpPost]
        [Route("enter_bidRoom")]
        public async Task<ActionResult> EnterBidRoom(string RoomCode, string UserCode)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.EnterBidRoom(RoomCode, UserCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpPost]
        [Route("update_bidRoom")]
        public async Task<ActionResult> UpdateBidRoom(string bidRoomCode, BidRoomDTO bidRoomDTO)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidRoomService.UpdateBidRoom(bidRoomCode, bidRoomDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

    }
}
