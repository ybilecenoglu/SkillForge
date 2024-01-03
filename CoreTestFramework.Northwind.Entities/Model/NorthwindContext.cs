using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace CoreTestFramework.Northwind.Entities.Model
{
    public class NorthwindContext : DbContext
    {
        //private readonly string _connString = "Data Source= ../CoreTestFramework.Northwind.Entities/Database/northwind.db";
        
        // public NorthwindContext(DbContextOptions<NorthwindContext> options): base (options)
        // {
            
        // }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Shipper> Shippers => Set<Shipper>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Territory> Territories => Set<Territory>();
        public DbSet<CustomerDemographic> CustomerDemographics => Set<CustomerDemographic>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Microsoft.Extensions.Configuration.Json sınıfı ile json dosyamızı okuttuk.
            IConfigurationRoot configuration = new ConfigurationBuilder()
            // .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../CoreTestFramework.Northwind.WebMvcUI/"))
            .AddJsonFile("appsettings.json")
            .Build();
            //Microsoft.EntityFrameworkCore.Proxies lazy loading işlemleri için ihtiyaç duyduğumuz sınıf.
            // optionsBuilder.UseLazyLoadingProxies().UseSqlite(configuration.GetConnectionString("NorthwindContext"));
             optionsBuilder.UseSqlite(configuration.GetConnectionString("NorthwindContext"));
        }
        // public override int SaveChanges()
        // {
        //     //Product Sınıfı için ef taraflı tracking state göre kaydetmeden önce yapılacaklar
        //     ChangeTracker.Entries().ToList().ForEach(e =>
        //     {
        //         if (e.Entity is Product product)
        //         {
        //             if (e.State == EntityState.Modified)
        //             {
        //                 product.Discontinued = "0";
        //             }
        //         }
        //     });
        //     return base.SaveChanges();
        // }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().Navigation(p => p.Products).AutoInclude();

            modelBuilder.Entity<Product>()
            .HasKey(p => p.ProductID);
            modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderDetails)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);
            // .OnDelete(DeleteBehavior.Restrict) ilişkili tablolarda veri silerken alınacak aksiyonu belirlediğimiz method

            modelBuilder.Entity<Product>().Navigation(p => p.Category).AutoInclude();
            modelBuilder.Entity<Product>().Navigation(p => p.Supplier).AutoInclude();
            //modelBuilder.Entity<Product>().Navigation(p => p.OrderDetails).AutoInclude();

            modelBuilder.Entity<Category>()
            .HasKey(c => c.CategoryID);
            modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(c=> c.CategoryId);

            modelBuilder.Entity<Supplier>()
            .HasKey(s => s.SupplierID);
            modelBuilder.Entity<Supplier>()
            .HasMany(s => s.Products)
            .WithOne(s => s.Supplier)
            .HasForeignKey(s => s.SupplierId);

            modelBuilder.Entity<Shipper>()
            .HasKey(s => s.ShipperID);
            modelBuilder.Entity<Shipper>()
            .HasMany(s => s.Orders)
            .WithOne(s => s.Shipper)
            .HasForeignKey(s => s.ShipVia);

            modelBuilder.Entity<Employee>()
            .HasKey( e => e.EmployeeID);
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.Employee)
            .HasForeignKey( e=> e.EmployeeId);
           
           //Many-to-many RelationShip
           modelBuilder.Entity<Employee>()
           .HasMany(e => e.Territories)
           .WithMany(e => e.Employees)
           .UsingEntity<Dictionary<string,object>>(
            "EmployeeTerritories",
            x => x.HasOne<Territory>().WithMany().HasForeignKey("EmployeeID"),
            x => x.HasOne<Employee>().WithMany().HasForeignKey("TerritoryID")
           );
           
            modelBuilder.Entity<Customer>()
            .HasKey(c => c.CustomerID);
            modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.CustomerId);
            
            modelBuilder.Entity<OrderDetail>(entity => {
                entity.HasKey(x => new {x.OrderId, x.ProductId});
            });
            
            modelBuilder.Entity<Territory>()
            .HasKey(t => t.TerritoryID);

            base.OnModelCreating(modelBuilder);
        }
        
    }
}
