using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Northwind.Entities.Concrate
{
    public class NorthwindContext : DbContext
    {
        private readonly string _connString = "Data Source= northwind.db";

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Navigation(p => p.Supplier).AutoInclude();
            modelBuilder.Entity<Product>().Navigation(p => p.Category).AutoInclude();
        }
    }
}
