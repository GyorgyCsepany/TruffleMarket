using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database.Mapping
{
    public class DepartmentMap : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Primary Key
            builder.HasKey(d => d.DepartmentId);

            // Table & Column Mappings
            builder.ToTable("Department");
            builder.Property(d => d.DepartmentId).HasColumnName("DepartmentId");
            builder.Property(d => d.DepartmentName).HasColumnName("DepartmentName");

        }
    }
}
