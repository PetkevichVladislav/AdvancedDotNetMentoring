using CatalogService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CatalogService.DataAccess.DatabaseContexts.MsSql
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public CatalogDbContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
