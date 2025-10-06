using SkillForge.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace SkillForge.Entities.Model
{
    public class NorthwindContext : DbContext
    {
        //private readonly string _connString = "Data Source= ../SkillForge.Entities/Database/northwind.db";
        
        // public NorthwindContext(DbContextOptions<NorthwindContext> options): base (options)
        // {
            
        // }
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Shipper> Shippers => Set<Shipper>();
        public virtual DbSet<Supplier> Suppliers => Set<Supplier>();
        public virtual DbSet<Category> Categories => Set<Category>();
        public virtual DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public virtual DbSet<Order> Orders => Set<Order>();
        public virtual DbSet<Customer> Customers => Set<Customer>();
        public virtual DbSet<Employee> Employees => Set<Employee>();
        public virtual DbSet<Territory> Territories => Set<Territory>();
        public virtual DbSet<CustomerDemographic> CustomerDemographics => Set<CustomerDemographic>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Microsoft.Extensions.Configuration.Json sınıfı ile json dosyamızı okuttuk.
            IConfigurationRoot configuration = new ConfigurationBuilder()
            // .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../SkillForge.WebMvcUI/"))
            .AddJsonFile("appsettings.json")
            .Build();
            
            //Microsoft.EntityFrameworkCore.Proxies lazy loading işlemleri için ihtiyaç duyduğumuz sınıf.
            // optionsBuilder.UseLazyLoadingProxies().UseSqlite(configuration.GetConnectionString("NorthwindContext"));
            
            //LogTo methodu ile ef core tarafından gönderilen sorguları logluyoruz.
             optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information).UseNpgsql(configuration.GetConnectionString("NorthwindContext")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //Global olarak tracking tanımı her seferinde AsNoTracking() methodunu kullanmamıza gerek kalmadı. 
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             //Product Sınıfı için ef taraflı tracking state göre kaydetmeden önce yapılacaklar
             var dateTime =DateTime.Now;
             var utcTime = new DateTime(dateTime.Year,dateTime.Month,dateTime.Day,dateTime.Hour,dateTime.Minute,dateTime.Second, DateTimeKind.Utc);
            ChangeTracker.Entries().ToList().ForEach(e =>
            {
                if (e.Entity is Product product)
                {
                    if (e.State == EntityState.Added) product.created_time = utcTime;
                    else if(e.State == EntityState.Modified) product.modified_time = utcTime;
                }
                else if(e.Entity is Category category){
                    if (e.State == EntityState.Added) category.created_time = utcTime;
                    else if(e.State == EntityState.Modified) category.modified_time = utcTime;
                }
            });
            return base.SaveChangesAsync(cancellationToken);
        }
        
        //Parametre alan function için kullanacağım IQueryable döndüren method
        //FromExpression verilen sorgu ifadesi için sorgulanabilir bir nesne oluştur.
        public IQueryable<OrderDetail> GetOrdersWithProductId(int productId) => FromExpression(() => GetOrdersWithProductId(productId));
        public IQueryable<OrderDTO> GetOrderWithOrderId(int orderId) 
        {
            return FromExpression(() => GetOrderWithOrderId(orderId));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TPT Yaklaşımı
            modelBuilder.Entity<Product>().ToTable("products", "public");
            modelBuilder.Entity<Category>().ToTable("categories", "public");
            modelBuilder.Entity<Supplier>().ToTable("suppliers", "public");
            modelBuilder.Entity<OrderDetail>().ToTable("order_details", "public");
            modelBuilder.Entity<Order>().ToTable("orders","public");
            modelBuilder.Entity<Employee>().ToTable("employees","public");
            modelBuilder.Entity<Customer>().ToTable("customers","public");
            modelBuilder.Entity<Shipper>().ToTable("shippers","public");
            // modelBuilder.Entity<Product>().HasIndex(p => p.ProductName).IncludeProperties(p => p.CategoryId) Database tarafında index oluşturma;
            // modelBuilder.Entity<Product>().HasCheckConstraint("PriceDiscountCheck","[Price]>[DiscountPrice]") Database taraflı kural belirleme Fiyat her zaman indirimli fiyattan büyük olmalıdır;
            modelBuilder.Entity<Product>()
            .HasKey(p => p.product_id);
            modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderDetails)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.product_id);
            //.OnDelete(DeleteBehavior.Restrict) ilişkili tablolarda veri silerken alınacak aksiyonu belirlediğimiz method
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.category_id);
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.supplier_id);
            modelBuilder.Entity<Supplier>()
            .HasKey(s => s.supplier_id);
            modelBuilder.Entity<Category>()
            .HasKey(c=> c.category_id);
            modelBuilder.Entity<Shipper>()
            .HasKey(s => s.shipper_id);
            modelBuilder.Entity<Shipper>()
            .HasMany(s => s.Orders)
            .WithOne(s => s.Shipper)
            .HasForeignKey(s => s.ship_via);
            modelBuilder.Entity<Employee>()
            .HasKey( e => e.employee_id);
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Orders)
            .WithOne(e => e.Employee)
            .HasForeignKey( e=> e.employee_id);
          
           //Many-to-many RelationShip
           modelBuilder.Entity<Employee>()
           .HasMany(e => e.Territories)
           .WithMany(e => e.Employees)
           .UsingEntity<Dictionary<string,object>>(
            "EmployeeTerritories",
            x => x.HasOne<Territory>().WithMany().HasForeignKey("employee_id"),
            x => x.HasOne<Employee>().WithMany().HasForeignKey("territory_id")
           );
           
           modelBuilder.Entity<Employee>()
           .HasKey(e => e.employee_id);
            modelBuilder.Entity<Customer>()
            .HasKey(c => c.customer_id);
            modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.customer_id);
            modelBuilder.Entity<OrderDetail>(entity => {
                entity.HasKey(x => new {x.order_id, x.product_id});
            });
            modelBuilder.Entity<Order>(entity => entity.HasKey(o => o.order_id));
            modelBuilder.Entity<Territory>()
            .HasKey(t => t.TerritoryID);
            
            //Function Execute
            //Parametre almayan function model builder execute
            // modelBuilder.Entity<OrderDTO>().ToFunction("fc_get_order_details_with_order_id");
            
            //Parametre Alan
            //Oluşturduğumuz function modelbuilder üzerinden execute ediyoruz.
            // modelBuilder.HasDbFunction(typeof(NorthwindContext).GetMethod(nameof(GetOrdersWithProductId), new [] {typeof(int)})).HasName("fc_get_orderdetails_with_productid");
            // modelBuilder.HasDbFunction(typeof(NorthwindContext).GetMethod(nameof(GetOrderWithOrderId), new [] {typeof(int)  
            // })).HasName("fc_get_order_details_with_order_id");
            
            //GLOBAL FILTER
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.is_deleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.is_deleted);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
