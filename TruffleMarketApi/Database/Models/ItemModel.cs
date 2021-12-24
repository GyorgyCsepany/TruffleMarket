namespace TruffleMarketApi.Database.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public int Price { get; set; }
        public string Origin { get; set; }
        public DateTimeOffset PickingDate { get; set; }
        public bool Certificated { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Description { get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }
        public bool ClosedBySeller { get; set; }
        public bool ClosedByBuyer { get; set; }

        public virtual User  Seller { get; set; }
        public virtual User  Buyer { get; set; }
    }
}
