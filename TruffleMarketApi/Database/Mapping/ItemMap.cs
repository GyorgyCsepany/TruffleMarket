using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database.Mapping
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            // Primary Key
            builder.HasKey(i => i.ItemId);

            // Table & Column Mappings
            builder.ToTable("Items");
            builder.Property(i => i.ItemId).HasColumnName("ItemId");
            builder.Property(i => i.Truffle).HasColumnName("Truffle");
            builder.Property(i => i.Weight).HasColumnName("Weight");
            builder.Property(i => i.Price).HasColumnName("Price");
            builder.Property(i => i.Origin).HasColumnName("Origin");
            builder.Property(i => i.DateOfPicking).HasColumnName("DateOfPicking");
            builder.Property(i => i.Certificated).HasColumnName("Certificated");
            builder.Property(i => i.Expiration).HasColumnName("Expiration");
            builder.Property(i => i.Seller).HasColumnName("Seller");

        }
    }
}