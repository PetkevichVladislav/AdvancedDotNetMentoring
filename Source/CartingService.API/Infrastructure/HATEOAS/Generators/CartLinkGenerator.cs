using CartingService.API.Infrastructure.HATEOAS.Models;
using CartingService.BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.API.Infrastructure.HATEOAS.Generators
{
    internal class CartLinkGenerator : ILinkGenerator<Cart>
    {
        private readonly IUrlHelper urlHelper;

        public CartLinkGenerator(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public IEnumerable<Link> GenerateLink(Cart cart)
        {
            var links = new List<Link>()
            {
                new Link
                {
                   Rel = "cart-self",
                   Method = "GET",
                   Href = urlHelper.Link("cart-self", new { cartId = cart.Id }),
                },
                new Link
                {
                   Rel = "cart-self-v2",
                   Method = "GET",
                   Href = urlHelper.Link("cart-self-v2", new { cartId = cart.Id }),
                },
                new Link
                {
                   Rel = "cart-add-line-item",
                   Method = "POST",
                   Href = urlHelper.Link("cart-add-line-item", new { cartId = cart.Id }),
                }
            };

            links.AddRange(cart.LineItems.Select(lineItem => new Link
            {
                Rel = $"cart-delete-line-item-{lineItem.Id}",
                Method = "DELETE",
                Href = urlHelper.Link("cart-delete-line-item", new { cartId = cart.Id, lineItemId = lineItem.Id }),
            }));

            return links;
        }
    }
}