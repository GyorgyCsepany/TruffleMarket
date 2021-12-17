using Microsoft.EntityFrameworkCore;
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

            var token = _jwtTokenService.GetToken(userProfile);

            userProfile.Token = token;

            return userProfile;
        }
    }
}
