using CartingService.BusinessLogicLayer.DTO;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using IdentityService.SDK.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.API.Controllers
{
    [ApiController]
    [ApiVersion("2")]
    [ApiVersion("1")]
    [Authorize(Roles = $"{Role.Manager},{Role.Buyer}")]
    [Route("/v{version:apiVersion}/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        /// <summary>
        /// Get <see cref="Cart"/> by <paramref name="cartId"/>.
        /// </summary>
        /// <param name="cartId">Cart primary key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Cart"/></returns>
        [MapToApiVersion("1")]
        [HttpGet("{cartId}", Name = "cart-self")]
        public async Task<ActionResult<Cart>> GetCartInfo(string cartId, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByIdAsync(cartId, cancellationToken);

            return cart is not null ? Ok(cart) : NotFound();
        }

        /// <summary>
        /// Get all <see cref="LineItem"/> from <see cref="Cart"/> by <paramref name="cartId"/>.
        /// </summary>
        /// <param name="cartId">Cart primary key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Collection of <see cref="LineItem"/>.</returns>
        [MapToApiVersion("2")]
        [HttpGet("{cartId}", Name = "cart-self-v2")]
        public async Task<ActionResult<Cart>> GetCartInfoV2(string cartId, CancellationToken cancellationToken)
        {
            var lineItems = await cartService.GetLineItemsAsync(cartId, cancellationToken);

            return lineItems.Any() ? Ok(lineItems) : NotFound();
        }

        /// <summary>
        /// Add <see cref="LineItem"/> to <see cref="Cart"/>.
        /// </summary>
        /// <param name="cartId">Cart primary key.</param>
        /// <param name="lineItem">Line item model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [MapToApiVersion("1")]
        [HttpPost("{cartId}/line-item", Name = "cart-add-line-item")]
        public async Task<ActionResult> AddLineItem(string cartId, [FromBody] LineItem lineItem, CancellationToken cancellationToken)
        {
            await cartService.AddLineItemAsync(cartId, lineItem, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Remove <see cref="LineItem"/> from cart by <paramref name="lineItemId"/>
        /// </summary>
        /// <param name="cartId">Cart primary key.</param>
        /// <param name="lineItemId">Line item unique identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [MapToApiVersion("1")]
        [HttpDelete("{cartId}/line-item/{lineItemId}", Name = "cart-delete-line-item")]
        public async Task<ActionResult> RemoveLineItem(string cartId, int lineItemId, CancellationToken cancellationToken)
        {
            await cartService.RemoveLineItemAsync(cartId, lineItemId, cancellationToken);

            return NoContent();
        }
    }
}