using Microsoft.EntityFrameworkCore;

using MongoDB.EntityFrameworkCore.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Masters.Domain.Entities;

namespace Masters.Infrastructure.Contexts
{
    public class MongoDbContext: DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToCollection("Products");
            modelBuilder.Entity<ProductType>().ToCollection("ProductTypes");
            modelBuilder.Entity<Customer>().ToCollection("Customers");

            //ProductType id value generated on add
            modelBuilder.Entity<ProductType>().Property(p => p.Id).ValueGeneratedOnAdd();

        }
    }
}
