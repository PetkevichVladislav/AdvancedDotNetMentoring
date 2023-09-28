using CatalogService.DataAccess.Models.Interfaces;

namespace CatalogService.DataAccess.Models
{
    public record Product : IEntity
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }

        public Uri? Image { get; init; }

        public decimal Price { get; init; }

        public uint Amount { get; init; }

        public int CategoryId { get; init; }

        public Category? Category { get; init; }
    }
}
