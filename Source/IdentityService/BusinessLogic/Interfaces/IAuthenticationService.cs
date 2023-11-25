using IdentityService.BusinessLogic.Models;

namespace IdentityService.BusinessLogic.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthorizationModel> LogIn(string username, string password);

        Task<AuthorizationModel> Register(string username, string password, string email);

        Task<AuthorizationModel> RefreshToken(string? accessToken, string? refreshToken);
    }
}
