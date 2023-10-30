using CartingService.API.Infrastructure.HATEOAS.Models;

namespace CartingService.API.Infrastructure.HATEOAS.Generators
{
    internal interface ILinkGenerator<TModel>
    {
       IEnumerable<Link> GenerateLink(TModel model);
    }
}
