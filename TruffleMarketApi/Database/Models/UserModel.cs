namespace TruffleMarketApi.Database.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
