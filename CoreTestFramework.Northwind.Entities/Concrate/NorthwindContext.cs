using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Northwind.Entities.Concrate
{
    public class NorthwindContext : DbContext
    {
        private readonly string _connString = "Data Source= northwind.db";

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers {get; set;}
        public DbSet<Category> Categories {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connString);
        }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Product>().ToTable("products","public");
        //     modelBuilder.Entity<Supplier>().ToTable("suppliers","public");
        //     modelBuilder.Entity<Category>().ToTable("categories","public");
        // }
    }
}
