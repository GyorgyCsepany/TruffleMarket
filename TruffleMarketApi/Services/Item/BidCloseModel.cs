namespace TruffleMarketApi.Services.Item
{
    public class BidCloseModel
    {
        public int ItemId { get; set; }
        public int SellerId { get; set; }
        public double? SellerRate { get; set; }
    }
}
