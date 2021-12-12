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

        public DbSet<Item> Items => Set<Item>();
        public DbSet<UserModel> Users => Set<UserModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
