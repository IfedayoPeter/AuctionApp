using AuctionApp.Domain.DTOs.Auction;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    [Authorize]
    [Route("api/v{version}/auction/[controller]")]
    //[Route("api/auction/[controller]")]
    [ApiController]
    public class AuctionController : BaseController
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost("create_auction")]
        public async Task<ActionResult> CreateAccount(AuctionDTO auctionDTO)
        {
            var result = new Result<AuctionDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.CreateAuction(auctionDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_auction_by_code")]
        public async Task<ActionResult> GetUserByCode(string auctionCode)
        {
            var result = new Result<AuctionDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.GetAuctionByCode(auctionCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_auctions")]
        public async Task<ActionResult> GetAllAuctions()
        {
            var result = new Result<List<AuctionDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.GetAllAuctions();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_active_auctions")]
        public async Task<ActionResult> GetActiveAuctions()
        {
            var result = new Result<List<AuctionDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.GetActiveAuctions();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_auction_results")]
        public async Task<ActionResult> GetAuctionResults()
        {
            var result = new Result<List<AuctionResultDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.GetAuctionResult();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("end_auctions")]
        public async Task<ActionResult> EndAuctons()
        {
            var result = new Result<List<AuctionDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.EndAuction();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("start_auctions")]
        public async Task<ActionResult> CheckAndStartAuctions()
        {
            var result = new Result<List<AuctionDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.CheckAndStartAuction();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpPut]
        [Route("update_auction")]
        public async Task<ActionResult> UpdateAuction(string auctionCode, UpdateAuctionDTO auctionDTO)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _auctionService.UpdateAuction(auctionCode, auctionDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

    }
}
