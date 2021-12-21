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

        public DbSet<ItemModel> Item => Set<ItemModel>();
        public DbSet<User> User => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
