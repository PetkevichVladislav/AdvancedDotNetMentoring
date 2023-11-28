using IdentityService.BusinessLogic.DataAccess;
using IdentityService.BusinessLogic.DataAccess.Models;
using IdentityService.BusinessLogic.DataAccess.Seeds;
using IdentityService.BusinessLogic.Interfaces;
using IdentityService.BusinessLogic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.BusinessLogic.Infrastructure
{
    public static class ServiceConfiguration
    {
        public async static Task AddIdentityServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.ConfigureIdentityDatabases(configuration);
            services.AddSingleton<ITokenService, TokenService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            await services.AddIdentityDefaultData();
        }

        public static async Task AddIdentityDefaultData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserWithRefreshToken>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleWithPermissions>>();
            await DefaultRoles.AddRoles(roleManager);
            await DefaultUsers.AddBuyer(userManager);
            await DefaultUsers.AddManager(userManager);
        }

        private static void ConfigureIdentityDatabases(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Identity")));
            services.AddIdentity<UserWithRefreshToken, RoleWithPermissions>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
