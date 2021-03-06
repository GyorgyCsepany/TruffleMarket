namespace TruffleMarketApi.Services.Item
{
    public class ItemRowModel
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public string Origin { get; set; }
        public DateTimeOffset PickingDate { get; set; }
        public DateTimeOffset Expiration { get; set; }

    }
}
