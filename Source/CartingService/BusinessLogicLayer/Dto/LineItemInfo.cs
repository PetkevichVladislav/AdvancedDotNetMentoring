namespace CartingService.BusinessLogicLayer.DTO
{
    public record LineItemInfo
    {
        public string? Name { get; init; }

        public string? ImageUrl { get; init; }

        public decimal Price { get; init; }
    }
}
