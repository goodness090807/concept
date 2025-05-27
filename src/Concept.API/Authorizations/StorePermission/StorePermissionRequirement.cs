using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.StorePermission
{
    public class StorePermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }
        public StorePermissionRequirement(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
            {
                throw new ArgumentException("使用StorePermissionRequirement，必須填寫PermissionName", nameof(permissionName));
            }

            PermissionName = permissionName;
        }
    }
}