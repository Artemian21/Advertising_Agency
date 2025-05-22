using Advertising_Agency.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.DiscountId);

            builder.Property(d => d.DiscountPercent)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(d => d.ValidFrom)
                .IsRequired();

            builder.Property(d => d.ValidTo)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            // Зв’язок Discount -> Service (багато до 1)
            builder.HasOne(d => d.Service)
                .WithMany(s => s.Discounts)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
