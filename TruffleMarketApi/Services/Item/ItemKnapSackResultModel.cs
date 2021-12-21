namespace TruffleMarketApi.Services.Item
{
    public class ItemKnapSackResultModel
    {
        public double TotalPrice { get; set; }
        public int TotalWeight { get; set; }
        public List<int> IncludedItemsId { get; set; } = new List<int>();
    }
}
 