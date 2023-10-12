namespace CartingService.BusinessLogicLayer.DTO
{
    public record LineItem
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public Image? Image { get; init; }

        public decimal Price { get; init; }

        public int Quantity { get; init;}
    }
}
