namespace CartingService.BusinessLogicLayer.DTO
{
    public record Cart
    {
        public string? Id { get; init; }

        public List<LineItem> LineItems { get; init; } = new List<LineItem>();
    }
}
