using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.PermissionAccess
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string permissionName)
        {
            PermissionName = permissionName;
            Policy = $"Permission:{permissionName}";
        }

        public string PermissionName { get; }
    }
}
