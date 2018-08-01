using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Almacen.Models.Data
{
    public class AlmacenDbContext : IdentityDbContext
    {
        public AlmacenDbContext(DbContextOptions<AlmacenDbContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(600);
            
        }

        public DbSet<Contains> Contains { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SupplierEnterprise> SupplierEnterprises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contains>().ToTable("Contain");
            modelBuilder.Entity<Make>().ToTable("Make");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<SupplierEnterprise>().ToTable("SupplierEnterprise");

            modelBuilder.Entity<Order>().HasOne(p => p.Client).WithMany(o => o.Orders);
            //modelBuilder.Entity<Order>().HasOne(p => p.Admin).WithMany(o => o.Orders);
        }
    }
}
