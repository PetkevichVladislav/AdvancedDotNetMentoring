using AutoMapper;
using CatalogService.BusinessLogic.Mapping.Profiles;

namespace CatalogService.BusinessLogic.Mapping
{
    public static class MapperProvider
    {
        public static MapperConfiguration mapperConfiguration = null!;

        private static void CreateMapping()
        {
            mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AllowNullDestinationValues = true;
                configuration.AddProfile<CategoryProfile>();
                configuration.AddProfile<ProductProfile>();
                configuration.AddProfile<MessagesProfile>();
            });
        }

        public static IMapper GetMapper()
        {
            if (mapperConfiguration == null)
            {
                CreateMapping();
            }

            return mapperConfiguration!.CreateMapper();
        }
    }
}
