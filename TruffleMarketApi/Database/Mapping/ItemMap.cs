using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database.Mapping
{
    public class ItemMap : IEntityTypeConfiguration<ItemModel>
    {
        public void Configure(EntityTypeBuilder<ItemModel> builder)
        {

            builder.HasKey(i => i.ItemId);

            builder.ToTable("Item");
            builder.Property(i => i.ItemId).HasColumnName("ItemId");
            builder.Property(i => i.Truffle).HasColumnName("Truffle");
            builder.Property(i => i.Weight).HasColumnName("Weight");
            builder.Property(i => i.Price).HasColumnName("Price").IsConcurrencyToken();
            builder.Property(i => i.Origin).HasColumnName("Origin");
            builder.Property(i => i.PickingDate).HasColumnName("PickingDate");
            builder.Property(i => i.Certificated).HasColumnName("Certificated");
            builder.Property(i => i.Expiration).HasColumnName("Expiration");
            builder.Property(i => i.Description).HasColumnName("Description");
            builder.Property(i => i.SellerId).HasColumnName("SellerId");
            builder.Property(i => i.BuyerId).HasColumnName("BuyerId");
            builder.Property(i => i.ClosedBySeller).HasColumnName("ClosedBySeller");

            builder
                .HasOne(i => i.Seller)
                .WithMany()
                .HasForeignKey(i => i.SellerId);

            builder
                .HasOne(i => i.Buyer)
                .WithMany()
                .HasForeignKey(i => i.BuyerId)
                .IsRequired(false);

        }
    }
}