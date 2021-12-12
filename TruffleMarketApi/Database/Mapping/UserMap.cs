using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database.Mapping
{
    public class UserMap : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            // Primary Key
            builder.HasKey(u => u.UserId);

            // Table & Column Mappings
            builder.ToTable("Users");
            builder.Property(i => i.UserId).HasColumnName("UserId");
            builder.Property(i => i.Name).HasColumnName("Name");
            builder.Property(i => i.Password).HasColumnName("Password");
            builder.Property(i => i.IsAdmin).HasColumnName("IsAdmin");

        }
    }
}
