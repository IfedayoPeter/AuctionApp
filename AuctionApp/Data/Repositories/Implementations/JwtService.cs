using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.Entities.User;
using Microsoft.IdentityModel.Tokens;

namespace AuctionApp.Data.Repositories.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(Login login)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Validissuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}