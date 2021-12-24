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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtAuthenticationConfig> _jwtAuthenticationConfig;

        public JwtTokenService(IHttpContextAccessor httpContextAccessor, IOptions<JwtAuthenticationConfig> jwtAuthenticationConfig)
        {
            _jwtAuthenticationConfig = jwtAuthenticationConfig;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken(UserProfileModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Name),
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Role, model.IsAdmin ? "Admin" : "User")
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


        public JwtSecurityToken ReadToken()
        { 
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken;
        }

    }
}
