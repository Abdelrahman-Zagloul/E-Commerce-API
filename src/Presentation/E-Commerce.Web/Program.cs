using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.Contexts;
using E_Commerce.Persistence.Data.DataSeed;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.Features;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Middlewares;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;

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
            await app.MigrateDataBase();
            await app.SeedDataBase();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
