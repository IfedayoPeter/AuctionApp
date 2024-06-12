using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers.V1
{
    [ApiController]
    [Route("api/v{version}/login/[controller]")]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }   

        [HttpPost("/UserLogin")]
        public async Task<IActionResult> UserLogin(LoginDTO loginDTO)
        {
            var result = await _loginService.UserLogin(loginDTO);
            return Ok(result);
        }

    }
}