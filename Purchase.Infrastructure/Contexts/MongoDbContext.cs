
using Microsoft.EntityFrameworkCore;

using MongoDB.EntityFrameworkCore.Extensions;

using Purchases.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Infrastructure.Contexts
{
    public class MongoDbContext: DbContext
    {

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }


        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Purchase>().ToCollection("Purchases");
            modelBuilder.Entity<PurchaseDetail>().ToCollection("PurchaseDetails");

        }
    }
}
