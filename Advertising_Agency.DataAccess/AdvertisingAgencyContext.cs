using Advertising_Agency.DataAccess.Configurations;
using Advertising_Agency.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Advertising_Agency.DataAccess
{
    public class AdvertisingAgencyContext : DbContext
    {
        public AdvertisingAgencyContext(DbContextOptions<AdvertisingAgencyContext> options)
            : base(options)
        {
        }

        // DbSet для кожної сутності
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Застосування конфігурацій через Fluent API
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
