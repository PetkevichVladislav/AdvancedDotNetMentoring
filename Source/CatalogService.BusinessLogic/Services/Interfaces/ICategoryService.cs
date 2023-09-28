namespace CatalogService.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<DTO.Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken);

        Task<List<DTO.Category>> GetAllAsync(CancellationToken cancellationToken);

        Task CreateAsync(DTO.Category category, CancellationToken cancellationToken);

        Task UpdateAsync(DTO.Category category, CancellationToken cancellationToken);

        Task DeleteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
