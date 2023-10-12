namespace CartingService.BusinessLogicLayer.DTO
{
    public record Image
    {
        public Uri? Url { get; init; }

        public string? AlternativeText { get; init; }
    }
}