using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TruffleMarketApi.Database;
using TruffleMarketApi.Services.Authentication;

namespace TruffleMarketApi.Services.User
{
    public class UserService: IUserService
    {
        private readonly TruffleMarketDbContext _dBContext;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(TruffleMarketDbContext dBContext, IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _dBContext = dBContext;
        }

        public async Task<UserProfileModel> LoginOrRegister(UserLoginOrRegisterModel model)
        {
            if (model is null)
                return null;

            Database.Models.User user;

            if (model.IsLogin)
            {                
                user = await _dBContext.User.FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);
   
                if (user is null)
                    return null;
            }
            else
            {
                user = new()
                {
                    Name = model.Name,
                    Password = model.Password,
                    Email = model.Email,
                };

                _dBContext.User.Add(user);
                await _dBContext.SaveChangesAsync();
            }

            var userProfile = new UserProfileModel
            {
                UserId = user.UserId,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
            };

            userProfile.Token = _jwtTokenService.GetToken(userProfile);

            return userProfile;
        }

        public async Task<int?> RateUser(int userId, double newRate)
        {
            var user = await _dBContext.User.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user is null)
                return null;

            var increasedRateCount = user.RateCount + 1;

            user.Rate = Math.Round(((user.Rate * user.RateCount) + newRate) / increasedRateCount, 2);
            user.RateCount = increasedRateCount;

            await _dBContext.SaveChangesAsync();

            return user.UserId;
        }

        public UserProfileModel UserProfil
        {
            get
            {
                var token = _jwtTokenService.ReadToken();

                if (token is null)
                    return null;

                var userId = int.Parse(token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var name = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                var role = token.Claims.First(c => c.Type == ClaimTypes.Role).Value;

                var userProfile = new UserProfileModel
                {
                    UserId = userId,
                    Name = name,
                    IsAdmin = role == "Admin",

                };

                return userProfile;
            }
        }
    }
}
