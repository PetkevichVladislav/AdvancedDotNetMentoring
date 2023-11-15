using AutoMapper;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using CartingService.DataAcessLayer.Repositories.Interfaces;

namespace CartingService.BusinessLogicLayer.Services
{
    public class LineItemService : ILineItemService
    {
        private readonly IMapper mapper;
        private readonly ICartRepository cartRepository;

        public LineItemService(ICartRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }

        public async Task UpdateLineItemByProductId(int productId, DTO.LineItemInfo lineItemInfo, CancellationToken cancellationToken = default)
        {
            var lineItemInfoModel = mapper.Map<MODELS.LineItemInfo>(lineItemInfo);
            await cartRepository.UpdateLineItemsByProductIdAsync(productId, lineItemInfoModel, cancellationToken);
        }
    }
}
