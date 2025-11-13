using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.Contexts;
using E_Commerce.Persistence.Data.DataSeed;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.Features;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Web.Extensions;
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

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
            });



            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(op =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });

            builder.Services.AddAutoMapper(typeof(IServicesAssemblyMarker).Assembly);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


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
