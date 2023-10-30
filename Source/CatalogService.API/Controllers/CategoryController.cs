using CatalogService.BusinessLogic.DTO;
using CatalogService.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.API.Controllers
{
    /// <summary>
    /// Controllers that allows to manipulate categories.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> logger;
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Creates instance of <see cref="CategoryController"/>
        /// </summary>
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            this.logger = logger;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Get all <see cref="Category"/>
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of categorires.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await categoryService.GetAllAsync(cancellationToken);

            return categories.Any() ? Ok(categories) : NotFound();
        }

        /// <summary>
        /// Get <see cref="Category"/> by id.
        /// </summary>
        /// <param name="id">Category primary key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Category"/> or null.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var category = await categoryService.GetByIdAsync(id, cancellationToken);

            return category is not null ? Ok(category) : NotFound();
        }

        /// <summary>
        /// Creates <see cref="Category"/>.
        /// </summary>
        /// <param name="category"><see cref="Category"/> to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Category"/></returns>
        [HttpPost]
        public async Task<ActionResult<Category>> AddAsync([FromBody] Category category, CancellationToken cancellationToken = default)
        {
            await categoryService.CreateAsync(category, cancellationToken);

            return Ok(category);
        }

        /// <summary>
        /// Updates <see cref="Category"/> by id.
        /// </summary>
        /// <param name="category"><see cref="Category"/> to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Category"/></returns>
        [HttpPut]
        public async Task<ActionResult<Category>> UpdateAsync([FromBody] Category category, CancellationToken cancellationToken = default)
        {
            await categoryService.CreateAsync(category, cancellationToken);

            return Ok(category);
        }

        /// <summary>
        /// Deletes <see cref="Category"/> by id.
        /// </summary>
        /// <param name="id">Category primary key.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Category"/></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}