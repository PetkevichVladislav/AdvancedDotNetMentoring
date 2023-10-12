using AutoMapper;
using CartingService.BusinessLogicLayer.Mapping.Profiles;

namespace CartingService.BusinessLogicLayer.Mapping
{
    public static class MapperProvider
    {
        public static MapperConfiguration mapperConfiguration = null!;

        private static void CreateMapping()
        {
            mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<CartProfile>();
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
