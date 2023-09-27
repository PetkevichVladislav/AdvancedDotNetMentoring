using CartingService.DataAcessLayer.Models;

namespace CartingService.DataAcessLayer.Interfaces
{
    public interface ICartRepository
    {
        Task<List<LineItem>> GetLineItemsAsync(int cartId, CancellationToken cancellationToken);

        Task AddLineItemAsync(int cartId, LineItem lineItem, CancellationToken cancellationToken);

        Task RemoveLineItemAsync(int cartId, int LineItemId, CancellationToken cancellationToken);
    }
}
