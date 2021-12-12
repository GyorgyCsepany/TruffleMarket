using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Services.Authentication
{
    public interface IJwtTokenService
    {
        Task<string> GetToken(UserModel userModel);
    }
}
