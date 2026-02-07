using Microsoft.EntityFrameworkCore;
using StockFlow.API.Models;
using System.Collections.Generic;

namespace StockFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Keyboard",
                    Price = 1500,
                    StockQuantity = 20
                },
                new Product
                {
                    Id = 2,
                    Name = "Mouse",
                    Price = 800,
                    StockQuantity = 50
                }
            );
        }
    }
}
