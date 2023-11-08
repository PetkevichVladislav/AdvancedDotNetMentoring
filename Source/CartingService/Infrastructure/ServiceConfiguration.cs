using AutoMapper;
using CartingService.BusinessLogicLayer.Mapping;
using CartingService.BusinessLogicLayer.Services;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using CartingService.DataAcessLayer.DatabaseContexts.MongoDb;
using CartingService.DataAcessLayer.Repositories.Interfaces;
using CartingService.DataAcessLayer.Repositories.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartingService.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static void AddCartingServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMongoDb(configuration);
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddSingleton(typeof(IMapper), MapperProvider.GetMapper());
        }

        private static void AddMongoDb(this IServiceCollection services, ConfigurationManager configuration)
        {
            var mongoDbSection = configuration.GetSection("MongoDB");
            var connectionString = mongoDbSection.GetSection("ConnectionString").Value;
            var databaseName = mongoDbSection.GetSection("DatabaseName").Value;
            services.AddSingleton(typeof(CartingDbContext), new CartingDbContext(connectionString!, databaseName!));
        }
    }
}
