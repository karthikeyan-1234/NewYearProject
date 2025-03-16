using Sales.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using MongoDB.EntityFrameworkCore.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Infrastructure.Contexts
{
    public class MongoDbContext: DbContext
    {
        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>().ToCollection("Sales");
            modelBuilder.Entity<SaleDetail>().ToCollection("SaleDetails");

            //ProductType id value generated on add
            modelBuilder.Entity<Sale>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SaleDetail>().Property(p => p.Id).ValueGeneratedOnAdd();

        }
    }
}
