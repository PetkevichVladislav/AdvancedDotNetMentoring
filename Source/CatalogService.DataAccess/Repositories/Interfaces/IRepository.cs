using CatalogService.DataAccess.Models.Interfaces;

namespace CatalogService.DataAccess.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        IQueryable<TEntity> GetAll();
    }
}
