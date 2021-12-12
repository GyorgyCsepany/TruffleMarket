using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TruffleMarketApi.Services.Authentication
{
    public static class AuthenticationExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var configSection = config.GetSection("JwtAuthentication");
            var jwtConfig = new JwtAuthenticationConfig();
            configSection.Bind(jwtConfig);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(c =>
                {
                    c.SaveToken = false;

                    c.RequireHttpsMetadata = true;

                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.ValidIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig.ValidAudience,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        RequireAudience = true
                    };
                });
        }
    }
}
