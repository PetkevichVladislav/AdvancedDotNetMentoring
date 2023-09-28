using CartingService.DataAcessLayer.DatabaseContexts.MongoDb;
using CartingService.DataAcessLayer.Models;
using CartingService.DataAcessLayer.Repositories.Interfaces;
using MongoDB.Driver;

namespace CartingService.DataAcessLayer.Repositories.Mongo
{
    internal class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> cartCollection;

        public CartRepository(CartingDbContext context)
        {
            this.cartCollection = context.GetCollection<Cart>(CollectionNames.CartCollection);
        }

        public async Task AddLineItemAsync(int cartId, LineItem lineItem, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(lineItem);
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);
            var existingLineItem = cart.LineItems.FirstOrDefault(existingLineItem => existingLineItem.Id == lineItem.Id);
            await UpdateAsync(cart, cancellationToken);
        }

        public async Task<List<LineItem>> GetLineItemsAsync(int cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);

            return cart.LineItems;
        }

        public async Task RemoveLineItemAsync(int cartId, int LineItemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);
            var lineItem = cart.LineItems.FirstOrDefault(existingLineItem => existingLineItem.Id == LineItemId);
            if (lineItem is not null)
            {
                cart.LineItems.Remove(lineItem);
                await UpdateAsync(cart, cancellationToken);
            }
        }

        private async Task<Cart> GetByIdAsync(int cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await cartCollection.Find(cart => cart.Id == cartId).FirstOrDefaultAsync(cancellationToken);

            return cart;
        }

        private async Task UpdateAsync(Cart cartToUpdate, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(cartToUpdate);

            await cartCollection.ReplaceOneAsync(cart => cart.Id == cartToUpdate.Id, cartToUpdate, cancellationToken: cancellationToken);
        }
    }
}
