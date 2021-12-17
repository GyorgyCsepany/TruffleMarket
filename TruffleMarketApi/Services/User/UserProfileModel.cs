namespace TruffleMarketApi.Services.User
{
    public class UserProfileModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
    }
}
