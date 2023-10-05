using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Northwind.Entities.Concrate
{
    public class NorthwindContext : DbContext
    {
        private readonly string _connString = "Server=localhost;Port=5432;Database=NorthwindDb;User Id=postgres;Password=12345;";
       
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers {get; set;}
        public DbSet<Category> Categories {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products","public");
            modelBuilder.Entity<Supplier>().ToTable("supplier","public");
            modelBuilder.Entity<Category>().ToTable("categories","public");
        }
    }
}
