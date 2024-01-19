using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityService.BusinessLogic.DataAccess.Models;
using IdentityService.BusinessLogic.Interfaces;
using IdentityService.BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<UserWithRefreshToken> userManager;
        private readonly RoleManager<RoleWithPermissions> roleManager;
        private readonly ITokenService tokenService;

        public AuthenticationService(UserManager<UserWithRefreshToken> userManager, RoleManager<RoleWithPermissions> roleManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
        }

        public async Task<AuthorizationModel> AuthenticateUser(string username, string password)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception("User with such username is not found");
            }

            var isPasswordIncorrect = !(await this.userManager.CheckPasswordAsync(user, password));
            if (isPasswordIncorrect)
            {
                throw new Exception("Password is incorrect");
            }

            var claims = await CreateClaims(user);
            var token = tokenService.CreateJwtToken(claims);
            var refreshToken = tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken.Value;
            user.RefreshTokenExpiryTime = refreshToken.ExpiryTime;
            await this.userManager.UpdateAsync(user);

            return new AuthorizationModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                AccessTokenExpiration = token.ValidTo
            };
        }

        public async Task<AuthorizationModel> CreateUser(string username, string password, string email)
        {
            var existingUser = await this.userManager.FindByNameAsync(username);
            if (existingUser is not null)
            {
                throw new Exception("User with such username already exists");
            }

            UserWithRefreshToken user = new()
            {
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username,
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception("Could not create user due to internal error.");
            }

            var authorizationModel = await AuthenticateUser(username, password);

            return authorizationModel;
        }

        public async Task<AuthorizationModel> RefreshToken(string? accessToken, string? refreshToken)
        {
            if (accessToken is null || refreshToken is null)
            {
                throw new Exception("Refresh or Access token is null");
            }

            var principal = tokenService.GetClaimPrincipalFromToken(accessToken);
            if (principal == null)
            {
                throw new Exception("Invalid access token or refresh token");
            }

            var user = await this.userManager.FindByNameAsync(principal.Identity.Name);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid access token or refresh token");
            }

            var newAccessToken = tokenService.CreateJwtToken(principal.Claims.ToList());
            var newRefreshToken = tokenService.CreateRefreshToken();

            user.RefreshToken = newRefreshToken.Value;
            await this.userManager.UpdateAsync(user);

            return new AuthorizationModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        private async Task<List<Claim>> CreateClaims(UserWithRefreshToken user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roleNames = await this.userManager.GetRolesAsync(user);
            claims.Add(new Claim("Roles", string.Join(",", roleNames)));
            foreach (var roleName in roleNames)
            {
                var role = await this.roleManager.FindByNameAsync(roleName);

                claims.Add(new Claim(ClaimTypes.Role, role.Name));

                var permissionClaims = role.Permissions.Select(permission => new Claim("identity/permissions", permission)).ToArray();
                claims.AddRange(permissionClaims);
            }

            return claims;
        }
    }
}
