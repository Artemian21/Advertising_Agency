using Advertising_Agency.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Advertising_Agency.DataAccess.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.OrderItemId);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.PriceAtOrder)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Зв’язок OrderItem -> Order (багато до 1)
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв’язок OrderItem -> Service (багато до 1)
            builder.HasOne(oi => oi.Service)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(oi => oi.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
