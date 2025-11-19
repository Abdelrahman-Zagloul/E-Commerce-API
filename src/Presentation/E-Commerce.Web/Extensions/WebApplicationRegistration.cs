using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.Contexts;
using E_Commerce.Persistence.IdentityData.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            await context.Database.MigrateAsync();

            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            await context.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDataBase(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await service.InitializeAsync();

            return app;
        }


    }
}
