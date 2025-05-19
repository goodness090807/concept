using Concept.Core.Entities.Permission;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        /// <summary>
        /// Check if a user has a specific permission, either directly or through roles
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="permissionName">The permission name</param>
        /// <returns>True if the user has the permission, otherwise false</returns>
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
        
        /// <summary>
        /// Get all permissions directly assigned to a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of directly assigned permissions</returns>
        Task<IEnumerable<PermissionEntity>> GetDirectPermissionsForUserAsync(int userId);
        
        /// <summary>
        /// Get all permissions assigned to a user through their roles
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of permissions from roles</returns>
        Task<IEnumerable<PermissionEntity>> GetRolePermissionsForUserAsync(int userId);
    }
}
