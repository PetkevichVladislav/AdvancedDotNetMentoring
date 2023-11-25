using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityService.BusinessLogic.Interfaces;
using IdentityService.BusinessLogic.Models;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<AuthorizationModel> LogIn(string username, string password)
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

            var claims = await CreateClaims(username, user);
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

        public async Task<AuthorizationModel> Register(string username, string password, string email)
        {
          var existingUser = await this.userManager.FindByNameAsync(username);
            if (existingUser is not null)
            {
                throw new Exception("User with such username already exists");
            }

            User user = new()
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

            var authorizationModel = await LogIn(username, password);

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

        private async Task<List<Claim>> CreateClaims(string username, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var roles = await this.userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roleClaims);

            return claims;
        }
    }
}
