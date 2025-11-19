using E_Commerce.Web.Extensions;
using E_Commerce.Web.Middlewares;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ApiServiceRegistration.AddApiServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseHttpsRedirection();


            await app.MigrateDataBaseAsync();
            await app.MigrateIdentityDataBaseAsync();

            await app.SeedDataBase();
            await app.SeedIdentityDataBase();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
