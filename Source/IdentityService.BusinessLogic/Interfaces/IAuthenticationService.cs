using IdentityService.BusinessLogic.Models;

namespace IdentityService.BusinessLogic.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthorizationModel> AuthenticateUser(string username, string password);

        Task<AuthorizationModel> CreateUser(string username, string password, string email);

        Task<AuthorizationModel> RefreshToken(string? accessToken, string? refreshToken);
    }
}
