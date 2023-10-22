namespace CartingService.BusinessLogicLayer.DTO
{
    public record Image
    {
        public string Url { get; init; }

        public string? AlternativeText { get; init; }
    }
}