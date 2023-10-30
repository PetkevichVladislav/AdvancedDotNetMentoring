using AutoMapper;
using CatalogService.BusinessLogic.Mapping;
using CatalogService.BusinessLogic.Services;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.DataAccess.Repositories.Interfaces;
using CatalogService.DataAccess.Repositories.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.BusinessLogic.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static void AddCatalogServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddSingleton(typeof(IMapper), MapperProvider.GetMapper());
        }
    }
}
