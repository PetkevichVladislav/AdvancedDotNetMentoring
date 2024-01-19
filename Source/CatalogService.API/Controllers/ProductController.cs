using CatalogService.BusinessLogic.DTO;
using CatalogService.BusinessLogic.Services.Interfaces;
using IdentityService.SDK.Infrastructure.Attributes;
using IdentityService.SDK.Models.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    /// <summary>
    /// Controllers that allows to manipulate line items.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly IProductService productService;

        /// <summary>
        /// Creates instance of <see cref="ProductController"/>
        /// </summary>
        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.logger = logger;
            this.productService = productService;
        }

        /// <summary>
        /// Get all products with pagination.
        /// </summary>
        /// <param name="categoryId">Filters products by linked category.</param>
        /// <param name="from">Takes categories from specified number.</param>
        /// <param name="to">Takes quantity of categories.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Collection with categories or empty.</returns>
        [Authorize]
        [Permission(CatalogPermission.Read)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync([FromQuery] int? categoryId, [FromQuery] int? from, [FromQuery] int? to, CancellationToken cancellationToken)
        {
            var products = await productService.GetAllWithFiltrationAndPaginationAsync(categoryId, from, to, cancellationToken);

            return products.Any() ? Ok(products) : NotFound();
        }

        /// <summary>
        /// Creates <see cref="Product"/>.
        /// </summary>
        /// <param name="product"><see cref="Product"/> to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Product"/></returns>
        [Authorize]
        [Permission(CatalogPermission.Create)]
        [HttpPost]
        public async Task<ActionResult<Product>> AddAsync([FromBody] Product product, CancellationToken cancellationToken)
        {
            await productService.CreateAsync(product, cancellationToken);

            return Ok(product);
        }

        /// <summary>
        /// Updates <see cref="Product"/> by id.
        /// </summary>
        /// <param name="product"><see cref="Product"/> to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Product"/></returns
        [Authorize]
        [Permission(CatalogPermission.Update)]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateAsync([FromBody] Product product, CancellationToken cancellationToken)
        {
            await productService.UpdateAsync(product, cancellationToken);

            return Ok(product);
        }

        /// <summary>
        /// Deletes <see cref="Product"/> by id.
        /// </summary>
        /// <param name="id">Product primary key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [Authorize]
        [Permission(CatalogPermission.Delete)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await productService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
