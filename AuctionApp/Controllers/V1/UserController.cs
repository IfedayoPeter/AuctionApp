using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Domain.Enums;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    //[Authorize]
    [Route("api/v{version}/user/[controller]")]
    //[Route("api/user/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create_buyer_account")]
        public async Task<ActionResult> CreateBuyerAccount(UserDTO userDTO)
        {
            var result = new Result<UserDTO>();
            result.RequestTime = DateTime.UtcNow;
            userDTO.UserCategory = UserCategoryEnum.Buyer;

            var response = await _userService.CreateAccount(userDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [HttpPost("create_seller_account")]
        public async Task<ActionResult> CreateSellerAccount(UserDTO userDTO)
        {
            var result = new Result<UserDTO>();
            result.RequestTime = DateTime.UtcNow;
            userDTO.UserCategory = UserCategoryEnum.Seller;

            var response = await _userService.CreateAccount(userDTO);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("get_buyers")]
        public async Task<ActionResult> GetBuyers()
        {
            var result = new Result<List<GetUserDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _userService.GetBuyers();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("get_sellers")]
        public async Task<ActionResult> GetSellers()
        {
            var result = new Result<List<GetUserDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _userService.GetSellers();
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("get_user_by_code")]
        public async Task<ActionResult> GetUserByCode(string UserCode)
        {
            var result = new Result<GetUserDTO>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _userService.GetUserByCode(UserCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("get_user_by_username")]
        public async Task<ActionResult> GetUserByUserName(string UserName)
        {
             var result = new Result<List<GetUserDTO>>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _userService.GetUserByUserName(UserName);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete_account")]
        public async Task<ActionResult> DeleteAccount(string UserCode)
        {
            var result = new Result<bool>();
            result.RequestTime = DateTime.UtcNow;

            var response = await _userService.DeleteUserAccount(UserCode);
            result = response;
            result.ResponseTime = DateTime.UtcNow;
            return Ok(result);
        }

    }
}
