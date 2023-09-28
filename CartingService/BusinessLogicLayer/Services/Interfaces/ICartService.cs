using CartingService.BusinessLogicLayer.DTO;

namespace CartingService.BusinessLogicLayer.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<LineItem>> GetLineItemsAsync(int cartId, CancellationToken cancellationToken);

        Task AddLineItemAsync(int cartId, LineItem lineItem, CancellationToken cancellationToken);

        Task RemoveLineItemAsync(int cartId, int LineIteemId, CancellationToken cancellationToken);
    }
}
