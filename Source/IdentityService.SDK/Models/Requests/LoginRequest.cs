namespace IdentityService.Models.Requests
{
    public record LoginRequest
    {
        public string? Username { get; init; }

        public string? Password { get; init; }
    }
}
