namespace CatalogService.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<DTO.Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken);

        Task<List<DTO.Category>> GetAllAsync(CancellationToken cancellationToken);

        Task<DTO.Category> CreateAsync(DTO.Category category, CancellationToken cancellationToken);

        Task<DTO.Category> UpdateAsync(DTO.Category category, CancellationToken cancellationToken);

        Task DeleteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
