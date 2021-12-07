using Microsoft.EntityFrameworkCore;
using TruffleMarketApi.Database.Mapping;
using TruffleMarketApi.Database.Models;

namespace TruffleMarketApi.Database
{
    public class TruffleMarketDbContext : DbContext
    {
        public TruffleMarketDbContext(DbContextOptions<TruffleMarketDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Department => Set<Department>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentMap());
        }
    }
}
