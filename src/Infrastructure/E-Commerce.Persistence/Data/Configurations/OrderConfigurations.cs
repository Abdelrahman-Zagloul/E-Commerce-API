using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, a =>
            {
                a.Property(x => x.Street).IsRequired();
                a.Property(x => x.City).IsRequired();
                a.Property(x => x.FirstName).IsRequired();
                a.Property(x => x.LastName).IsRequired();
                a.Property(x => x.Country).IsRequired();

            });

            builder.Property(o => o.SubTotal)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(o => o.DeliveryMethod)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(o => o.DeliveryMethodId);
        }
    }
}
