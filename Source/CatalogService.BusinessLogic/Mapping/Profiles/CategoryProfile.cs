using AutoMapper;

namespace CatalogService.BusinessLogic.Mapping.Profiles
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<DTO.Category, MODELS.Category>()
            .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory : null))
            .ReverseMap();
        }
    }
}
