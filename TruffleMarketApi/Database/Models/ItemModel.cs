﻿namespace TruffleMarketApi.Database.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string Truffle { get; set; }
        public int Weight { get; set; }
        public double Price { get; set; }
        public string Origin { get; set; }
        public DateTimeOffset PickingDate { get; set; }
        public bool Certificated { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Description { get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }

        public User Seller { get; set; }
        public User Buyer { get; set; }
    }
}
