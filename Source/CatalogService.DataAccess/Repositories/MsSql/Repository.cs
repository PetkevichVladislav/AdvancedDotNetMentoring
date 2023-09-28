using CatalogService.DataAccess.DatabaseContexts.MsSql;
using CatalogService.DataAccess.Models.Interfaces;
using CatalogService.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DataAccess.Repositories.MsSql
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly CatalogDbContext catalogContext;

        public Repository(CatalogDbContext catalogContext)
        {
            this.catalogContext = catalogContext;

        }
        public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);
            cancellationToken.ThrowIfCancellationRequested();

            await this.catalogContext.Set<TEntity>().AddAsync(entity);
            await this.catalogContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await GetByIdAsync(id, cancellationToken);
            this.catalogContext.Set<TEntity>().Remove(entity);
            await this.catalogContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            var result = this.catalogContext.Set<TEntity>().AsNoTracking();

            return result;
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await this.catalogContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id == id);

            return result!;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);
            cancellationToken.ThrowIfCancellationRequested();

            this.catalogContext.Set<TEntity>().Update(entity);
            await this.catalogContext.SaveChangesAsync();
        }
    }
}
