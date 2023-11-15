using CartingService.BusinessLogicLayer.DTO;
using FluentValidation;

namespace CartingService.BusinessLogicLayer.Validators
{
    internal class ImageValidator : AbstractValidator<Image?>
    {
        public ImageValidator()
        {
            RuleFor(image => image.AlternativeText).NotEmpty();
            RuleFor(image => image.Url).Must(url => Uri.IsWellFormedUriString(url?.ToString(), UriKind.Absolute));
        }
    }
}
