using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.ToTable("UserList");
            builder.Property(i => i.UserId).HasColumnName("UserId");
            builder.Property(i => i.Name).HasColumnName("Name");
            builder.Property(i => i.Password).HasColumnName("Password");
            builder.Property(i => i.IsAdmin).HasColumnName("IsAdmin");
            builder.Property(i => i.Email).HasColumnName("Email");
            builder.Property(i => i.Rate).HasColumnName("Rate");
            builder.Property(i => i.RateCount).HasColumnName("RateCount");

        }
    }
}
