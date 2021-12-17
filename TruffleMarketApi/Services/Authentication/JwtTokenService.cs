using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TruffleMarketApi.Services.User;

namespace TruffleMarketApi.Services.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<JwtAuthenticationConfig> _jwtAuthenticationConfig;

        public JwtTokenService(IOptions<JwtAuthenticationConfig> jwtAuthenticationConfig)
        {
            _jwtAuthenticationConfig = jwtAuthenticationConfig;
        }

        public string GetToken(UserProfileModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name),
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Role, model.IsAdmin ? "Administrator" : "User")
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

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
