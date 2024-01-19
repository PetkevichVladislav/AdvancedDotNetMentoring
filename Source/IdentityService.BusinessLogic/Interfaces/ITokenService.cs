using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityService.BusinessLogic.Models;
using IdentityService.BusinessLogic.Models.Enums;

namespace IdentityService.BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateJwtToken(List<Claim> claims);

        RefreshToken CreateRefreshToken();

        ClaimsPrincipal GetClaimPrincipalFromToken(string? token);

        TokenValidationResult ValidateToken(string accessToken);

        string GetPublicKey();
    }
}
