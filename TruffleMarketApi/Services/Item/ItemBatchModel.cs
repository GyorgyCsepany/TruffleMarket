namespace TruffleMarketApi.Services.Item
{
    public class ItemBatchModel
    {
        public int BuyerId { get; set; }
        public double MaxTotalPrice { get; set; }
        public int MinTotalWeight { get; set; }
        public string Truffle {get; set;}
    }
}
