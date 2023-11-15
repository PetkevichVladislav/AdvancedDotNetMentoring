using AutoMapper;
using CartingService.BusinessLogicLayer.DTO;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using CartingService.BusinessLogicLayer.Validators;
using CartingService.DataAcessLayer.Repositories.Interfaces;

namespace CartingService.BusinessLogicLayer.Services
{
    public class CartService : ICartService
    {
        private readonly IMapper mapper;
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }

        public async Task AddLineItemAsync(string cartId, DTO.LineItem lineItem, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(lineItem);

            ValidateLineItem(lineItem);
            var lineItemModel = mapper.Map<MODELS.LineItem>(lineItem);
            await this.cartRepository.AddLineItemAsync(cartId, lineItemModel, cancellationToken);
        }

        public async Task<List<DTO.LineItem>> GetLineItemsAsync(string cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lineItemModels = await this.cartRepository.GetLineItemsAsync(cartId, cancellationToken);
            var lineItems = mapper.Map<List<DTO.LineItem>>(lineItemModels);

            return lineItems;
        }

        public async Task<DTO.Cart> GetCartByIdAsync(string cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var cartModel = await this.cartRepository.GetByIdAsync(cartId, cancellationToken);
            var cart = mapper.Map<DTO.Cart>(cartModel);

            return cart;
        }

        public async Task RemoveLineItemAsync(string cartId, int lineIteemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.cartRepository.RemoveLineItemAsync(cartId, lineIteemId, cancellationToken);
        }

        private void ValidateLineItem(LineItem lineItem)
        {
            var validator = new LineItemValidator();
            var validationResult = validator.Validate(lineItem);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException($"Line item is not valid.Errors: {validationResult.Errors}");
            }
        }
    }
}
