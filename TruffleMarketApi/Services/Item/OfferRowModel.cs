namespace TruffleMarketApi.Services.Item
{
    public class OfferRowModel
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public ItemStatus Status { get; set; }
    }
}
