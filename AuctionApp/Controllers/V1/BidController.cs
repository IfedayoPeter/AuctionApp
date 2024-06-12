using AuctionApp.Domain.DTOs.Bid;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    [Authorize]
    [Route("api/v{version}/bid/[controller]")]
    //[Route("api/bid/[controller]")]
    [ApiController]
    public class BidController : BaseController
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost("submit_bid")]
        public async Task<ActionResult> SubmitBid(BidDTO bidDTO)
        {
            var result = new Result<BidDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidService.SubmitBid(bidDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_bid_by_code")]
        public async Task<ActionResult> GetBidByCode(string bidCode)
        {
            var result = new Result<BidDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidService.GetBidByCode(bidCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_bids")]
        public async Task<ActionResult> GetAllBids()
        {
            var result = new Result<List<BidDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidService.GetAllBids();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpGet]
        [Route("get_highest_bids")]
        public async Task<ActionResult> GetHighestBids(string auctionCode)
        {
            var result = new Result<BidDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidService.GetHighestBid(auctionCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        
        [HttpPut]
        [Route("update_bid")]
        public async Task<ActionResult> UpdateBid(string bidCode, UpdateBidDTO bidDTO)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _bidService.UpdateBid(bidCode, bidDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

    }
}
