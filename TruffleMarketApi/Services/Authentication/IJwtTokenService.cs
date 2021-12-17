using TruffleMarketApi.Services.User;

namespace TruffleMarketApi.Services.Authentication
{
    public interface IJwtTokenService
    {
        string GetToken(UserProfileModel model);
    }
}
