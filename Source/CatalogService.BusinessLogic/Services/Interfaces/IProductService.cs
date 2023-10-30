namespace CatalogService.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<DTO.Product> GetByIdAsync(int productId, CancellationToken cancellationToken);

        Task<List<DTO.Product>> GetAllWithFiltrationAndPaginationAsync(int? categoryId, int? from, int? to, CancellationToken cancellationToken);

        Task CreateAsync(DTO.Product product, CancellationToken cancellationToken);

        Task UpdateAsync(DTO.Product product, CancellationToken cancellationToken);

        Task DeleteAsync(int productId, CancellationToken cancellationToken);
    }
}
