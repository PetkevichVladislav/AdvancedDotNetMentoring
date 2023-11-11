using AutoMapper;

namespace CartingService.BusinessLogicLayer.Mapping.Profiles
{
    internal class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<DTO.Cart, MODELS.Cart>()
                .ReverseMap();

            CreateMap<DTO.LineItem, MODELS.LineItem>()
                .ReverseMap();

            CreateMap<DTO.LineItemInfo, MODELS.LineItemInfo>()
                .ReverseMap();

            CreateMap<DTO.Image, MODELS.Image>()
                .ReverseMap();
        }
    }
}
