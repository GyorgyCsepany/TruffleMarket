namespace TruffleMarketApi.Database.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string Email { get; set; }
        public double? Rate { get; set; }
        public int RateCount { get; set; } = 0;
    }
}
