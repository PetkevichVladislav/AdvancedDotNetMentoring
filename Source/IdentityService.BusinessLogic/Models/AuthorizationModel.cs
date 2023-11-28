namespace IdentityService.BusinessLogic.Models
{
    public record AuthorizationModel
    {
        public string? AccessToken { get; init; }

        public DateTime? AccessTokenExpiration { get; init; }

        public RefreshToken? RefreshToken { get; init; }
    }
}
