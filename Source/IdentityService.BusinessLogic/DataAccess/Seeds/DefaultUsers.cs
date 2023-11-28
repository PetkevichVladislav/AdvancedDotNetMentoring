using IdentityService.BusinessLogic.DataAccess.Models;
using IdentityService.SDK.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.DataAccess.Seeds
{
    public static class DefaultUsers
    {
        public static async Task AddBuyer(UserManager<UserWithRefreshToken> userManager)
        {
            var defaultUser = new UserWithRefreshToken
            {
                UserName = "Buyer",
                Email = "buyer@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Role.Buyer);
                }
            }
        }

        public static async Task AddManager(UserManager<UserWithRefreshToken> userManager)
        {
            var defaultUser = new UserWithRefreshToken
            {
                UserName = "manager",
                Email = "manager@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Role.Manager);
                }
            }
        }
    }
}
