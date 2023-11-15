using CartingService.DataAcessLayer.DatabaseContexts.MongoDb;
using CartingService.DataAcessLayer.Models;
using CartingService.DataAcessLayer.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CartingService.DataAcessLayer.Repositories.Mongo
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> cartCollection;

        public CartRepository(CartingDbContext context)
        {
            this.cartCollection = context.GetCollection<Cart>(CollectionNames.CartCollection);

            BsonClassMap.RegisterClassMap<LineItem>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(c => c.Price).SetSerializer(new DecimalSerializer(BsonType.Decimal128));
            });
        }

        public async Task AddLineItemAsync(string cartId, LineItem lineItem, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(lineItem);
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);
            if (cart is null)
            {
                cart = new Cart
                {
                    Id = cartId,
                };

                await CreateAsync(cart, cancellationToken);
            }

            cart.LineItems.Add(lineItem);
            await UpdateAsync(cart, cancellationToken);
        }

        public async Task<List<LineItem>> GetLineItemsAsync(string cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);

            return cart.LineItems;
        }

        public async Task UpdateLineItemsByProductIdAsync(int productId, LineItemInfo lineItemInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<Cart>.Filter.ElemMatch(cart => cart.LineItems, item => item.ProductId == productId);
            var update = Builders<Cart>.Update.Set("LineItems.$.Name", lineItemInfo.Name)
                                              .AddToSet("LineItems.$.Price", lineItemInfo.Price)
                                              .AddToSet("LineItems.$.Image.Url", lineItemInfo.ImageUrl);

            await cartCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        }

        public async Task RemoveLineItemAsync(string cartId, int lineItemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cart = await GetByIdAsync(cartId, cancellationToken);
            var lineItem = cart.LineItems.FirstOrDefault(existingLineItem => existingLineItem.Id == lineItemId);
            if (lineItem is not null)
            {
                cart.LineItems.Remove(lineItem);
                await UpdateAsync(cart, cancellationToken);
            }
        }

        public async Task<Cart> GetByIdAsync(string cartId, CancellationToken cancellationToken)
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

        private async Task CreateAsync(Cart cart, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(cart);

            await cartCollection.InsertOneAsync(cart, cancellationToken: cancellationToken);
        }
    }
}
