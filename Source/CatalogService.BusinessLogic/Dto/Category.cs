namespace CatalogService.BusinessLogic.DTO
{
    public record Category
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public Uri? Image { get; init; }

        public int? ParentCategoryId { get; init; }

        public Category? ParentCategory { get; init; } = null;
    }
}
