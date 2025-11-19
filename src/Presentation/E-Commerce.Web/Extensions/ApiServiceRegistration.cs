using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Persistence.Data.Contexts;
using E_Commerce.Persistence.Data.DataSeed;
using E_Commerce.Persistence.IdentityData.Contexts;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.Features;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;
namespace E_Commerce.Web.Extensions
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureControllerAndSwagger(services);
            AddDbContext(services, configuration);
            AddIdentityDbContext(services, configuration);
            RegisterServices(services, configuration);


            return services;
        }

        private static void ConfigureControllerAndSwagger(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CreateValidationErrorResponse;
            });
        }


        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalConnection"));
            });
        }
        private static void AddIdentityDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalIdentityConnection"));
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;

            })
            .AddEntityFrameworkStores<StoreIdentityDbContext>();

        }
        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICasheRepository, CasheRepository>();
            services.AddScoped<ICasheService, CasheService>();
            services.AddSingleton<IConnectionMultiplexer>(
                op => ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!)
            );

            services.AddAutoMapper(typeof(IServicesAssemblyMarker).Assembly);
        }
    }
}
