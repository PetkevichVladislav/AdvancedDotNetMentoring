using AutoMapper;
using MessgingService.Models.Messages;

namespace CartingService.BusinessLogicLayer.Mapping.Profiles
{
    internal class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<UpdateProductMessage, DTO.LineItemInfo>()
                .ForMember(dto => dto.ImageUrl, conf => conf.MapFrom(message => message.Image));
        }
    }
}
