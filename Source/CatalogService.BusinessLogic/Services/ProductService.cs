using AutoMapper;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.BusinessLogic.Validators;
using CatalogService.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<MODELS.Product> productRepository;
        private readonly IMapper mapper;

        public ProductService(IRepository<MODELS.Product> productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<DTO.Product> GetByIdAsync(int productId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var productModel = await this.productRepository.GetByIdAsync(productId, cancellationToken);
            var result = this.mapper.Map<DTO.Product>(productModel);

            return result;
        }

        public async Task<List<DTO.Product>> GetAllWithFiltrationAndPaginationAsync(int? categoryId, int? skip, int? take, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var productModelsExpression = this.productRepository.GetAll().Where(product => categoryId == null || product.CategoryId == categoryId.Value);

            if (skip is not null)
            {
                productModelsExpression.Skip(skip.Value);
            }

            if (take is not null)
            {
                productModelsExpression.Take(take.Value);
            }

            var productModels = await productModelsExpression.ToListAsync();
            var result = this.mapper.Map<List<DTO.Product>>(productModels);

            return result;
        }

        public async Task CreateAsync(DTO.Product product, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(product);

            Validate(product);
            var productModel = this.mapper.Map<MODELS.Product>(product);
            await this.productRepository.CreateAsync(productModel, cancellationToken);
        }

        public async Task UpdateAsync(DTO.Product product, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(product);

            Validate(product);
            var productModel = this.mapper.Map<MODELS.Product>(product);
            await this.productRepository.UpdateAsync(productModel, cancellationToken);
        }

        public async Task DeleteAsync(int productId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await this.productRepository.DeleteAsync(productId, cancellationToken);
        }

        private void Validate(DTO.Product product)
        {
            var validator = new ProductValidator();
            var validationResult = validator.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException($"Product is not valid. Errors: {validationResult.Errors}");
            }
        }
    }
}
