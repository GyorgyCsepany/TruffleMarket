namespace TruffleMarketApi.Services.User
{
    public class UserLoginOrRegisterModel
    {
        public bool IsLogin { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
