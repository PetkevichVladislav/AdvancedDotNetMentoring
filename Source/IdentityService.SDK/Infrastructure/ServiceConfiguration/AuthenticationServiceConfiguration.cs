using System.Security.Cryptography;
using IdentityService.SDK.Infrastructure.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.SDK.Infrastructure.ServiceConfiguration
{
    public static class AuthenticationServiceConfiguration
    {
        public static void UseJwtLoggingMiddleware(this WebApplication application)
        {
            application.UseMiddleware<JwtLoggingMiddleware>();
        }

        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, ConfigurationManager configuration)
        {

            RSA validationKey = GetValidationKey(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    IssuerSigningKey = new RsaSecurityKey(validationKey),
                };
            });

            services.AddAuthorization();
        }

        private static RSA GetValidationKey(ConfigurationManager configuration)
        {
            using var client = new HttpClient();
            var url = configuration["AuthenticationService:BaseUrl"] + configuration["AuthenticationService:Authentication:GetPublicKey"];
            var result = client.GetAsync(url).Result;
            var validationKey = RSA.Create();
            var publicKey = result.Content.ReadAsStringAsync().Result;
            validationKey.ImportFromPem(publicKey);

            return validationKey;
        }
    }
}
