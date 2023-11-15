using CartingService.BusinessLogicLayer.DTO;
using FluentValidation;

namespace CartingService.BusinessLogicLayer.Validators
{
    internal class CartValidator : AbstractValidator<Cart>
    { 
        public CartValidator()
        {
            RuleForEach(cart => cart.LineItems).SetValidator(new LineItemValidator());
        }
    }
}
