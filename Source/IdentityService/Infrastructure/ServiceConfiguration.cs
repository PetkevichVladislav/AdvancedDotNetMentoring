using System.Text;
using IdentityService.BusinessLogic.Interfaces;
using IdentityService.BusinessLogic.Services;
using IdentityService.DataAccess;
using IdentityService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CartingService.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static void AddIdentityServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.ConfigureIdentityDatabases(configuration);
            services.ConfigureAuthentication(configuration);
        }

        private static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            var jwtConfigurationSection = configuration.GetSection("JWT");
            var jwtSecret = jwtConfigurationSection.GetValue<string>("Secret")!;
            var jwtIssuer = jwtConfigurationSection.GetValue<string>("ValidIssuer")!;
            var jwtAudience = jwtConfigurationSection.GetValue<string>("ValidAudience")!;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = jwtAudience,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });
        }

        private static void ConfigureIdentityDatabases(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Identity")));
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
