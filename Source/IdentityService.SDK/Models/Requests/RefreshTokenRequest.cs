namespace IdentityService.Models.Requests
{
    public record RefreshTokenRequest
    {
        public string? AccessToken { get; init; }

        public string? RefreshToken { get; init; }
    }
}
