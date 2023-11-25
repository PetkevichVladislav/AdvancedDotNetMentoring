namespace IdentityService.Models
{
    public record RegisterRequest
    {
        public string? Email { get; init; }

        public string? Username { get; init; }

        public string? Password { get; init; }

    }
}
