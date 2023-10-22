using CartingService.BusinessLogicLayer.DTO;

namespace CartingService.BusinessLogicLayer.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<LineItem>> GetLineItemsAsync(string cartId, CancellationToken cancellationToken);

        Task AddLineItemAsync(string cartId, LineItem lineItem, CancellationToken cancellationToken);

        Task RemoveLineItemAsync(string cartId, int lineIteemId, CancellationToken cancellationToken);

        Task<Cart> GetCartByIdAsync(string cartId, CancellationToken cancellationToken);
    }
}
