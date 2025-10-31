using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(StoreDbContext context, ILogger<DbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            bool hasTypes = await _context.ProductTypes.AnyAsync();
            bool hasBrands = await _context.ProductBrands.AnyAsync();
            bool hasProducts = await _context.Products.AnyAsync();

            if (hasTypes && hasBrands && hasProducts)
                return;

            try
            {
                if (!hasTypes)
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _context.ProductTypes);
                if (!hasBrands)
                    await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _context.ProductBrands);

                await _context.SaveChangesAsync();

                if (!hasProducts)
                    await SeedDataFromJsonAsync<Product, int>("products.json", _context.Products);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed data");
                return;
            }
        }
        private async Task SeedDataFromJsonAsync<TEntity, TKey>(string fileName, DbSet<TEntity> dbSet) where TEntity : BaseEntity<TKey>
        {
            try
            {
                var filePath = @"..\..\..\src\Infrastructure\E-Commerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Seed data file not found: {filePath}");

                using var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data != null && data.Any())
                    await dbSet.AddRangeAsync(data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding data from {FileName}", fileName);
                throw;
            }
        }
    }
}
