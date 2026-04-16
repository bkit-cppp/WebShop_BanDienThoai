using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebEcomerceStoreAPI.Entities;

namespace WebEcomerceStoreAPI.Data
{
    public partial class StoreDbContext :DbContext
    {
        public StoreDbContext()
        {

        }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).AddEnvironmentVariables().Build();
            return configuration.GetConnectionString("DefaultConnectionString");
        }
      public DbSet<Product> Products { get; set; }
      public DbSet<ProductImages> ProductImages { get; set; }
      public DbSet<Category> Categories { get; set; }
      public DbSet<User> Users { get; set; }
      public DbSet<Order> Orders { get; set; }
      public DbSet<Inventory> Inventories { get; set; }
      public DbSet<OrderDetail> OrderDetails { get; set; }
      public DbSet<Payment> Payments { get; set; }
      public DbSet<Roles> Roles { get; set; }
      public DbSet<DisCountCode> DisCountCodes { get; set; }
      public DbSet<Reviews> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("Category");
                entity.Property(c => c.CategoryId).IsRequired();
                entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Description).IsRequired().HasMaxLength(200);
                
                
            });
            modelBuilder.Entity<DisCountCode>(entity =>
            {
                entity.HasKey(e => e.DiscountId);
                entity.ToTable("DisCountCode");
                entity.Property(c => c.Code).IsRequired().HasMaxLength(200);
                entity.Property(c => c.DiscountAmount).IsRequired().HasMaxLength(200);
                entity.Property(c => c.DiscountPercent).IsRequired().HasMaxLength(200);
            });
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.InventoryId);
                entity.ToTable("Inventory");
                entity.Property(c => c.Quantity).IsRequired().HasMaxLength(200);
                entity.Property(c => c.LastDated).IsRequired();
                entity.HasOne(entity => entity.Products).WithMany(i => i.Inventories).HasForeignKey(e => e.ProductId).
                OnDelete(DeleteBehavior.ClientSetNull);
        

            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.ToTable("OrderDetail");
                entity.Property(c => c.Quantity).IsRequired().HasMaxLength(200);
                entity.Property(c => c.UnitPrice).IsRequired();
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.ToTable("Order");
                entity.Property(c => c.OrderDate).IsRequired();
                entity.Property(c => c.Status).IsRequired();

            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.ToTable("Payment");
                entity.Property(c => c.PaymentDate).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Status).IsRequired();
                entity.Property(c => c.Amount).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.ToTable("Product");
                entity.Property(c => c.Description).IsRequired().HasMaxLength(200);
                entity.Property(c => c.QuantityStock).IsRequired();
                entity.Property(c => c.Type).IsRequired();
                entity.Property(c => c.Brand).IsRequired();
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.PictureUrl).IsRequired();
                entity.Property(c => c.Price).IsRequired();
                entity.HasOne(entity=>entity.Category).WithMany(p => p.Products).
                HasForeignKey(e=>e.CategoryId).OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<ProductImages>(entity =>
            {
                entity.HasKey(e => e.ImageId);
                entity.ToTable("ProductImages");
                entity.Property(c => c.ImageUrl).IsRequired();
                entity.Property(c => c.IsMain).IsRequired();
                
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("User");
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.Status).IsRequired();
                entity.Property(u => u.Address).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(e => e.ReviewId);
                entity.ToTable("Reviews");
                entity.Property(u => u.Comment).IsRequired();
                entity.Property(u => u.CreatedDate).IsRequired();
                entity.Property(u => u.Rating).IsRequired();
            });
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.ToTable("Roles");
                entity.Property(u => u.RoleName).IsRequired();

            });
            modelBuilder.Entity<Roles>().HasData(
             new Roles { RoleId = 1, RoleName = "Admin" },
             new Roles { RoleId = 2, RoleName = "User" }
             );
            base.OnModelCreating(modelBuilder);
        }
    }
}
