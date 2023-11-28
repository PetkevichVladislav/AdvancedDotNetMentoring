using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityService.BusinessLogic.Interfaces;
using IdentityService.BusinessLogic.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TokenValidationResult = IdentityService.BusinessLogic.Models.Enums.TokenValidationResult;

namespace IdentityService.BusinessLogic.Services
{
    public class TokenService : ITokenService
    {
        private readonly string jwtSecret;
        private readonly int jwtTokenTermInMinutes;
        private readonly int refreshTokenTermInDays;
        private readonly string jwtIssuer;
        private readonly string jwtAudience;
        private readonly RSAParameters privateKey;
        private readonly RSAParameters publicKey;
        private readonly string publicKeyPem;

        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly TokenValidationParameters tokenValidationParameters;

        public TokenService(IConfiguration configuration)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            var jwtConfigurationSection = configuration.GetSection("JWT");
            jwtSecret = jwtConfigurationSection.GetValue<string>("Secret")!;
            jwtTokenTermInMinutes = jwtConfigurationSection.GetValue<int>("TokenValidityInMinutes")!;
            refreshTokenTermInDays = jwtConfigurationSection.GetValue<int>("RefreshTokenValidityInDays")!;
            jwtIssuer = jwtConfigurationSection.GetValue<string>("ValidIssuer")!;
            jwtAudience = jwtConfigurationSection.GetValue<string>("ValidAudience")!;

            using RSA rsa = RSA.Create();
            this.privateKey = rsa.ExportParameters(true);
            this.publicKey = rsa.ExportParameters(false);
            this.publicKeyPem = rsa.ExportRSAPublicKeyPem();

            tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(publicKey),
                ValidateLifetime = false,
            }; 
        }

        public JwtSecurityToken CreateJwtToken(List<Claim> claims)
        {
            var signingKey = new RsaSecurityKey(privateKey);
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                expires: DateTime.Now.AddMinutes(jwtTokenTermInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
            );

            return token;
        }

        public RefreshToken CreateRefreshToken()
        {
            var token = new RefreshToken
            {
                Value = GenerateRefreshTokenValue(),
                ExpiryTime = DateTime.Now.AddDays(refreshTokenTermInDays),
            };

            return token;
        }

        public string GetPublicKey()
        {
            return publicKeyPem;
        }

        public TokenValidationResult ValidateToken(string token)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKeyPem);

                var validationKey = new RsaSecurityKey(rsa);
                tokenValidationParameters.IssuerSigningKey = validationKey;

                try
                {
                    tokenValidationParameters.ValidateLifetime = true;
                    var jwt = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                    return TokenValidationResult.Valid;
                }
                catch (SecurityTokenExpiredException exception)
                {
                    return TokenValidationResult.Expired;
                }
                catch (SecurityTokenException exception)
                {
                    return TokenValidationResult.Invalid;
                }
                catch (Exception exception)
                {
                    return TokenValidationResult.Invalid;
                }
            }
        }

        public ClaimsPrincipal GetClaimPrincipalFromToken(string? token)
        {
            var claimPrincipal = this.tokenHandler.ValidateToken(token, this.tokenValidationParameters, out SecurityToken securityToken);

            return claimPrincipal;
        }

        private static string GenerateRefreshTokenValue()
        {
            RandomNumberGenerator rng;
            var randomNumber = new byte[64];
            rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
