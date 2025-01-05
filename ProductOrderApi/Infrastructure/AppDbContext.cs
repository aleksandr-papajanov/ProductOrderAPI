using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Bcpg.Sig;
using ProductOrderApi.Entities;
using System.Reflection.Emit;

namespace ProductOrderApi.Infrastructure
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Order> Products { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderTracking> OrderTracking { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<GoogleAuthState> GoogleAuthStates { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region products

            builder.Entity<ProductFeature>(e =>
            {
                e.HasKey(e => new { e.ProductId, e.FeatureId });

                e.HasData(
                    new ProductFeature { ProductId = 1, FeatureId = 1, Value = "Red" },
                    new ProductFeature { ProductId = 1, FeatureId = 2, Value = "50mm" },
                    new ProductFeature { ProductId = 1, FeatureId = 3, Value = "12.455 kg" },

                    new ProductFeature { ProductId = 2, FeatureId = 1, Value = "Greed" },
                    new ProductFeature { ProductId = 2, FeatureId = 2, Value = "200mm" },
                    new ProductFeature { ProductId = 2, FeatureId = 3, Value = "0.455 kg" },

                    new ProductFeature { ProductId = 3, FeatureId = 1, Value = "Blue" },
                    new ProductFeature { ProductId = 3, FeatureId = 2, Value = "1010mm" },
                    new ProductFeature { ProductId = 3, FeatureId = 3, Value = "1.12 kg" }
                );
            });

            builder.Entity<Feature>(e =>
            {
                e.HasIndex(e => e.Name)
                 .IsUnique();

                e.HasData(
                    new Feature { Id = 1, Name = "Color" },
                    new Feature { Id = 2, Name = "Length" },
                    new Feature { Id = 3, Name = "Weight" }
                );
            });

            builder.Entity<Product>(e =>
            {
                e.HasMany(e => e.Features)
                 .WithOne(e => e.Product)
                 .HasForeignKey(e => e.ProductId);

                e.HasMany(e => e.Orders)
                 .WithOne(e => e.Product)
                 .HasForeignKey(e => e.ProductId);

                e.HasIndex(e => e.Code)
                 .IsUnique();

                e.HasIndex(e => e.Name)
                 .IsUnique();

                e.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Product_Price", "Price > 0");
                    t.HasCheckConstraint("CK_Product_QuantityInStock", "QuantityInStock >= 0");
                });

                e.HasData(
                    new Product { Id = 1, Code = "AD268754", Name = "Product 1", Description = "Test description for product 1", QuantityInStock = 14, Price = 15.99M, CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now, IsAvailable = true },
                    new Product { Id = 2, Code = "FR235467", Name = "Product 2", Description = "Test description for product 2", QuantityInStock = 3, Price = 1.50M, CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now, IsAvailable = true },
                    new Product { Id = 3, Code = "TY547756", Name = "Product 3", Description = "Test description for product 3", QuantityInStock = 11, Price = 24.99M, CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now, IsAvailable = true }
                );
            });

            #endregion

            #region orders

            builder.Entity<OrderTracking>(e =>
            {
                e.HasOne(e => e.Order)
                 .WithMany(e => e.Tracking)
                 .HasForeignKey(e => e.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_OrderTracking_Status", "Status BETWEEN 1 AND 8");
                });

                e.HasData(
                    new OrderTracking { Id = 1, OrderId = 1, Status = OrderStatus.Created, UpdatedAt = DateTime.Now.AddDays(-3) },
                    new OrderTracking { Id = 2, OrderId = 1, Status = OrderStatus.Confirmed, UpdatedAt = DateTime.Now.AddDays(-2) },
                    new OrderTracking { Id = 3, OrderId = 1, Status = OrderStatus.Processing, UpdatedAt = DateTime.Now.AddDays(-1) },

                    new OrderTracking { Id = 4, OrderId = 2, Status = OrderStatus.Created, UpdatedAt = DateTime.Now.AddDays(-5) },
                    new OrderTracking { Id = 5, OrderId = 2, Status = OrderStatus.Confirmed, UpdatedAt = DateTime.Now.AddDays(-4) },
                    new OrderTracking { Id = 6, OrderId = 2, Status = OrderStatus.Processing, UpdatedAt = DateTime.Now.AddDays(-3) },
                    new OrderTracking { Id = 7, OrderId = 2, Status = OrderStatus.Cancelled, UpdatedAt = DateTime.Now.AddDays(-2) },

                    new OrderTracking { Id = 8, OrderId = 3, Status = OrderStatus.Created, UpdatedAt = DateTime.Now.AddDays(0) }
                );
            });

            builder.Entity<OrderProduct>(e =>
            {
                e.HasKey(op => new { op.OrderId, op.ProductId });

                e.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_OrderProduct_Quantity", "Price > 0");
                    t.HasCheckConstraint("CK_OrderProduct_Price", "Price > 0");
                });

                e.HasData(
                    new OrderProduct { OrderId = 1, ProductId = 1, Quantity = 1, Price = 11.51M },
                    new OrderProduct { OrderId = 1, ProductId = 2, Quantity = 2, Price = 15.20M },
                    new OrderProduct { OrderId = 1, ProductId = 3, Quantity = 3, Price = 1234.19M },

                    new OrderProduct { OrderId = 2, ProductId = 2, Quantity = 1, Price = 1451.60M },
                    new OrderProduct { OrderId = 2, ProductId = 3, Quantity = 2, Price = 1321.12M },

                    new OrderProduct { OrderId = 3, ProductId = 2, Quantity = 14, Price = 151.42M }

                );
            });

            builder.Entity<Order>(e =>
            {
                e.HasOne(e => e.User)
                 .WithMany(e => e.Orders)
                 .HasForeignKey(e => e.UserId);

                e.HasMany(e => e.Cart)
                 .WithOne(e => e.Order)
                 .HasForeignKey(e => e.OrderId);

                e.HasMany(e => e.Tracking)
                 .WithOne(e => e.Order)
                 .HasForeignKey(e => e.OrderId);

                e.HasData(
                    new Order { Id = 1, Comment = "Test order 1", TotalPrice = 133.14M, UserId = 1 },
                    new Order { Id = 2, Comment = "Test order 2", TotalPrice = 444.12M, UserId = 2 },
                    new Order { Id = 3, Comment = "Test order 3", TotalPrice = 234.12M, UserId = 3 }
                );
            });

            #endregion

            builder.Entity<User>(e =>
            {
                e.HasMany(e => e.Roles)
                 .WithOne(e => e.User)
                 .HasForeignKey(e => e.UserId)
                 .IsRequired();

                e.HasIndex(e => e.Email)
                 .IsUnique();

                // all passwords are "string"
                e.HasData(
                    new User { Id = 1, Email = "admin@example.com", Password = "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL", IsActive = true },
                    new User { Id = 2, Email = "customer@example.com", Password = "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL", IsActive = true },
                    new User { Id = 3, Email = "admin.custumer@example.com", Password = "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL", IsActive = true }
                );
            });

            builder.Entity<UserRole>(e =>
            {
                e.HasKey(op => new { op.UserId, op.Role });

                e.HasData(
                    new UserRole { UserId = 1, Role = "Admin" },
                    new UserRole { UserId = 2, Role = "Customer" },
                    new UserRole { UserId = 3, Role = "Admin" },
                    new UserRole { UserId = 3, Role = "Customer" }
                );
            });

            builder.Entity<UserConfirmationToken>(e =>
            {
                e.HasKey(op => new { op.UserId, op.Token });

                e.HasOne(c => c.User)
                 .WithOne(u => u.ConfirmationToken)
                 .HasForeignKey<UserConfirmationToken>(c => c.UserId);

                e.HasKey(op => new { op.UserId, op.Token });
            });

            builder.Entity<GoogleAuthState>(e =>
            {
                e.HasKey(e => e.State);
            });
        }
    }
}