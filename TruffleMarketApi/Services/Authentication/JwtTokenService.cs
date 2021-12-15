using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TruffleMarketApi.Database.Models;
using TruffleMarketApi.Models;

namespace TruffleMarketApi.Services.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<JwtAuthenticationConfig> _jwtAuthenticationConfig;

        public JwtTokenService(IOptions<JwtAuthenticationConfig> jwtAuthenticationConfig)
        {
            _jwtAuthenticationConfig = jwtAuthenticationConfig;
        }

        public UserResponseModel GetUserWithToken(UserModel user)
        {
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

            var userResponse = new UserResponseModel
            {
                UserId = user.UserId,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt)
            };

            return userResponse;
        }
    }
}
