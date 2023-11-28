using Microsoft.AspNetCore.Identity;

namespace IdentityService.BusinessLogic.DataAccess.Models
{
    public class RoleWithPermissions : IdentityRole<int>
    {
        public string[] Permissions { get; set; }
    }
}
