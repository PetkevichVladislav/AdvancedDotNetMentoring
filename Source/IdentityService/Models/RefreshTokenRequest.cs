namespace IdentityService.Models
{
    public record RefreshTokenRequest
    {
        public string? AccessToken { get; init; }

        public string? RefreshToken { get; init; }
    }
}
