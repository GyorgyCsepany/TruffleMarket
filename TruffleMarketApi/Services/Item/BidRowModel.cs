namespace TruffleMarketApi.Services.Item
{
    public class BidRowModel
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public double? SellerRate { get; set; }
        public string SellerEmail { get; set; }
        public bool IsExecuted { get; set; }  
    }
}
