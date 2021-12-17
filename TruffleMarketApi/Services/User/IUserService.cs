namespace TruffleMarketApi.Services.User
{
    public interface IUserService
    {
        Task<UserProfileModel> LoginOrRegister(UserLoginOrRegisterModel model);
    }
}
