using CartingService.DataAcessLayer.Models;

namespace CartingService.DataAcessLayer.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<List<LineItem>> GetLineItemsAsync(string cartId, CancellationToken cancellationToken);

        Task AddLineItemAsync(string cartId, LineItem lineItem, CancellationToken cancellationToken);

        Task RemoveLineItemAsync(string cartId, int lineItemId, CancellationToken cancellationToken);

        Task UpdateLineItemsByProductIdAsync(int productId, LineItemInfo lineItemInfo, CancellationToken cancellationToken);

        Task<Cart> GetByIdAsync(string cartId, CancellationToken cancellationToken);
    }
}
