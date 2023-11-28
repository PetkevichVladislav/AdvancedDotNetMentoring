using IdentityService.BusinessLogic.DataAccess.Models;
using IdentityService.SDK.Models.Permissions;
using IdentityService.SDK.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.DataAccess.Seeds
{
    internal static class DefaultRoles
    {
        public static async Task AddRoles(RoleManager<RoleWithPermissions> roleManager)
        {
            await roleManager.CreateAsync(new RoleWithPermissions
            {
                Name = Role.Buyer,
                Permissions = new[]
                {
                    CatalogPermission.Read,  
                }
            });
            await roleManager.CreateAsync(new RoleWithPermissions
            {
                Name = Role.Manager,
                Permissions = new[]
                {
                    CatalogPermission.Read,
                    CatalogPermission.Create,
                    CatalogPermission.Delete,
                    CatalogPermission.Update,
                }
            });
        }
    }
}
