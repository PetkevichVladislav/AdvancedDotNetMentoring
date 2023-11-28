using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.DataAccess.Models
{
    public class UserWithRefreshToken : IdentityUser<int>
    {
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
