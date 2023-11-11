using CatalogService.BusinessLogic.DTO;
using FluentValidation;

namespace CatalogService.BusinessLogic.Validators
{
    internal class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name).NotEmpty().MaximumLength(50);
            RuleFor(product => product.CategoryId).NotNull();
            RuleFor(product => product.Price).NotNull();
            RuleFor(product => product.Amount).NotNull();
        }
    }
}
