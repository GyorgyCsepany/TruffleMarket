using TruffleMarketApi.Database.Models;
using TruffleMarketApi.Models;

namespace TruffleMarketApi.Services.Authentication
{
    public interface IJwtTokenService
    {
        UserResponseModel GetUserWithToken(UserModel userModel);
    }
}
