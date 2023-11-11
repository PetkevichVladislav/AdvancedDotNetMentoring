using AutoMapper;
using MessgingService.Models.Messages;

namespace CatalogService.BusinessLogic.Mapping.Profiles
{
    internal class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            CreateMap<DTO.Product, UpdateProductMessage>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ReverseMap();
        }
    }
}
