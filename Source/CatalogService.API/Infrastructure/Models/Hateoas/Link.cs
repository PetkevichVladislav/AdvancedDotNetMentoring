namespace CatalogService.API.Infrastructure.Models.Hateoas
{
    public record Link
    {
        public string? Href { get; init; }
        public string? Rel { get; init; }
        public string? Method { get; init; }
    }
}
