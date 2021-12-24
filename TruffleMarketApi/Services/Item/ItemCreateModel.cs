namespace TruffleMarketApi.Services.Item
{
    public class ItemCreateModel
    {
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public string Origin { get; set; }
        public DateTimeOffset PickingDate { get; set; }
        public bool Certificated { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Description { get; set; }
    }
}
