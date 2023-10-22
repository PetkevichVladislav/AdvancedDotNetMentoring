using AutoMapper;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.BusinessLogic.Validators;
using CatalogService.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<MODELS.Category> categoryRepository;
        private readonly IRepository<MODELS.Product> productRepository;
        private readonly IMapper mapper;

        public CategoryService(IRepository<MODELS.Category> categoryRepository, IRepository<MODELS.Product> productRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public async Task<DTO.Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var categoryModel = await this.categoryRepository.GetByIdAsync(categoryId, cancellationToken);
            var result = this.mapper.Map<DTO.Category>(categoryModel);

            return result;
        }

        public async Task<List<DTO.Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var categoryModels = await this.categoryRepository.GetAll().ToListAsync(cancellationToken);
            var result = this.mapper.Map<List<DTO.Category>>(categoryModels);

            return result;
        }

        public async Task CreateAsync(DTO.Category category, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(category);

            Validate(category);
            var categoryModel = this.mapper.Map<MODELS.Category>(category);
            await this.categoryRepository.CreateAsync(categoryModel, cancellationToken);
        }

        public async Task UpdateAsync(DTO.Category category, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(category);

            Validate(category);
            var categoryModel = this.mapper.Map<MODELS.Category>(category);
            await this.categoryRepository.UpdateAsync(categoryModel, cancellationToken);
        }

        public async Task DeleteAsync(int categoryId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.categoryRepository.DeleteAsync(categoryId, cancellationToken);
            var productIds = await this.productRepository.GetAll()
                .Where(product => product.CategoryId == categoryId)
                .Select(product => product.Id)
                .ToArrayAsync(cancellationToken);
            foreach (var productId in productIds)
            {
                await this.productRepository.DeleteAsync(productId, cancellationToken);
            }
        }

        private void Validate(DTO.Category category)
        {
            var validator = new CategoryValidator();
            var validationResult = validator.Validate(category);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException($"Category is not valid. Errors: {validationResult.Errors}");
            }
        }
    }
}
