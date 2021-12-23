namespace TruffleMarketApi.Services.User
{
    public interface IUserService
    {
        Task<UserProfileModel> LoginOrRegister(UserLoginOrRegisterModel model);
        Task<int?> RateUser(int userId, double newRate);
    }
}
