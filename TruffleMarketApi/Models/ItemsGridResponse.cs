using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Models
{
    public class ItemsGridResponse
    {
        public int TotalRows { get; set; }
        public List<Item> Rows { get; set; }
    }
}
