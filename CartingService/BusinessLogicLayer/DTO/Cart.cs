namespace CartingService.BusinessLogicLayer.DTO
{
    public record Cart
    {
        public int Id { get; init; }

        public List<LineItem> LineItems { get; init; } = new List<LineItem>();
    }
}
