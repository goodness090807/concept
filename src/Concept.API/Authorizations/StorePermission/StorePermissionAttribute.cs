using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.StorePermission
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class StorePermissionAttribute : AuthorizeAttribute
    {
        public StorePermissionAttribute(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new ArgumentException("使用StorePermissionAttribute，必須填寫PermissionName", nameof(permissionName));
            }

            Policy = $"StorePermission:{permissionName}";
        }
    }
}