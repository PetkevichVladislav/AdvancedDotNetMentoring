using AutoMapper;

namespace CatalogService.BusinessLogic.Mapping.Profiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<DTO.Product, MODELS.Product>()
                .ReverseMap()
                .ForAllMembers(options => options.Condition((source, destination, member) => member != null));
        }
    }
}
