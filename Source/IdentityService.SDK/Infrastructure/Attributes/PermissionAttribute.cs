using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityService.SDK.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;

        public PermissionAttribute(string[] permissions)
        {
            _permissions = permissions;
        }
        public PermissionAttribute(string permission)
        {
            _permissions = new string[] { permission };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasPermission = context.HttpContext.User.Claims.Any(claim => claim.Type == "identity/permissions" && _permissions.Contains(claim.Value));
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
