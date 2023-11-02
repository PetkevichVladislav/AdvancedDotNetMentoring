using CartingService.BusinessLogicLayer.DTO;
using FluentValidation;

namespace CartingService.BusinessLogicLayer.Validators
{
    internal class LineItemValidator : AbstractValidator<LineItem>
    {
        public LineItemValidator()
        {
            RuleFor(lineItem => lineItem.Name).NotEmpty();
            RuleFor(lineItem => lineItem.Image).SetValidator(new ImageValidator());
        }
    }
}
