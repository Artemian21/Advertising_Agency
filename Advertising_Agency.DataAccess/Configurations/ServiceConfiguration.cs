using Advertising_Agency.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Advertising_Agency.DataAccess.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(s => s.ServiceId);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Description)
                .HasMaxLength(1000);

            builder.Property(s => s.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // enum ServiceType зберігаємо як string
            builder.Property(s => s.ServiceType)
                .HasConversion<string>()
                .IsRequired();

            // Зв’язок Service -> OrderItems (1 до багатьох)
            builder.HasMany(s => s.OrderItems)
                .WithOne(oi => oi.Service)
                .HasForeignKey(oi => oi.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв’язок Service -> Discounts (1 до багатьох)
            builder.HasMany(s => s.Discounts)
                .WithOne(d => d.Service)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
