
using Microsoft.EntityFrameworkCore;

using MongoDB.EntityFrameworkCore.Extensions;

using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Contexts
{
    public class MongoDbContext: DbContext
    {

        public DbSet<InventoryEntry> InventoryEntries { get; set; }


        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryEntry>().ToCollection("InventoryEntries");
            //id value generated on add
            modelBuilder.Entity<InventoryEntry>().Property(p => p.Id).ValueGeneratedOnAdd();

        }
    }
}
