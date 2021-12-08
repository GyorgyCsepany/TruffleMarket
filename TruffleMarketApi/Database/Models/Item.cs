namespace TruffleMarketApi.Database.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }
        public string Origin { get; set; }
        public DateTimeOffset DateOfPicking { get; set; }
        public bool Certificated { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Seller { get; set; }
    }
}
