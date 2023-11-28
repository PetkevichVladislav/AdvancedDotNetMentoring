namespace IdentityService.BusinessLogic.Models
{
    public record RefreshToken
    {
        public string? Value { get; init; }
        
        public DateTime ExpiryTime { get; init; }
    }
}
