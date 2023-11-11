using CartingService.BusinessLogicLayer.DTO;

namespace CartingService.BusinessLogicLayer.Services.Interfaces
{
    public interface ILineItemService
    {
        Task UpdateLineItemByProductId(int productId, LineItemInfo lineItemInfo, CancellationToken cancellationToken = default);
    }
}
