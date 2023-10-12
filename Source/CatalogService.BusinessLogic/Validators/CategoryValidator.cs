using CatalogService.BusinessLogic.DTO;
using FluentValidation;

namespace CatalogService.BusinessLogic.Validators
{
    internal class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Name).NotEmpty().MaximumLength(50);
            RuleFor(category => category.ParentCategory).SetValidator(this!);
        }
    }
}
