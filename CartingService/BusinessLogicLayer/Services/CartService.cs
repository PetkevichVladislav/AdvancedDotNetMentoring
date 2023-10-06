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

        public async Task AddLineItemAsync(int cartId, DTO.LineItem lineItem, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(lineItem);

            ValidateLineItem(lineItem);
            var lineItemModel = mapper.Map<MODELS.LineItem>(lineItem);
            await this.cartRepository.AddLineItemAsync(cartId, lineItemModel, cancellationToken);
        }

        

        public async Task<List<DTO.LineItem>> GetLineItemsAsync(int cartId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lineItemModels = await this.cartRepository.GetLineItemsAsync(cartId, cancellationToken);
            var lineItems = mapper.Map<List<DTO.LineItem>>(lineItemModels);

            return lineItems;
        }

        public async Task RemoveLineItemAsync(int cartId, int LineIteemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.cartRepository.RemoveLineItemAsync(cartId, LineIteemId, cancellationToken);
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
