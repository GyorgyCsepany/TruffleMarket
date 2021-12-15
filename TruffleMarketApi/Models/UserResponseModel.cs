namespace TruffleMarketApi.Models
{
    public class UserResponseModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
    }
}
