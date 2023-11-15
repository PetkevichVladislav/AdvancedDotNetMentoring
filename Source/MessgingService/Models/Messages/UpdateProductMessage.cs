namespace MessgingService.Models.Messages
{
    public record UpdateProductMessage : Message
    {
        public int ProductId { get; set; }

        public string? Name { get; init; }

        public Uri? Image { get; init; }

        public decimal Price { get; init; }
    }
}
