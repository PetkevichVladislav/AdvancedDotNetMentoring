namespace CartingService.API.Infrastructure.HATEOAS.Models
{
    internal class Resource
    {
        public object? Data { get; set; }

        public IEnumerable<Link> Links { get; set; }
    }
}
