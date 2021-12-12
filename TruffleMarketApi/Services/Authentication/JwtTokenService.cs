using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TruffleMarketApi.Database;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Services.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly TruffleMarketDbContext _truffleMarketDbContext;
        private readonly IOptions<JwtAuthenticationConfig> _jwtAuthenticationConfig;

        public JwtTokenService(TruffleMarketDbContext truffleMarketDbContext, IOptions<JwtAuthenticationConfig> jwtAuthenticationConfig)
        {
            _truffleMarketDbContext = truffleMarketDbContext;
            _jwtAuthenticationConfig = jwtAuthenticationConfig;
        }

        public async Task<string> GetToken(UserModel userModel)
        {
            var user = await _truffleMarketDbContext.Users.FirstOrDefaultAsync(u => u.Name == userModel.Name && u.Password == userModel.Password);

            if (user is null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Administrator" : "User")
            };

            var secret = _jwtAuthenticationConfig.Value.Secret;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.AddHours(8);

            var jwt = new JwtSecurityToken
            (
                issuer: _jwtAuthenticationConfig.Value.ValidIssuer,
                audience: _jwtAuthenticationConfig.Value.ValidAudience,
                claims: claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return jwtToken;

        }
    }
}
