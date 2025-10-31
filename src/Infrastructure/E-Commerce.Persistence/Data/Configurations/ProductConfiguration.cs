using E_Commerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasColumnType("nvarchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.PictureUrl)
                .HasColumnType("nvarchar")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();


            builder.HasOne(p => p.ProductBrand)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();


            builder.HasOne(p => p.ProductType)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
