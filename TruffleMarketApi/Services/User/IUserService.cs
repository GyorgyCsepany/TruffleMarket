namespace TruffleMarketApi.Services.User
{
    public interface IUserService
    {
        Task<UserResponseModel> LoginOrRegister(UserLoginOrRegisterModel model);
        UserProfileModel UserProfil { get; }
        Task<int?> RateUser(int userId, double newRate);
    }
}
