using E_Commerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations
{
    internal class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.ToTable("ProductBrands");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
