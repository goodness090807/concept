using Concept.Core.Entities.Permission;

namespace Concept.Core.Interfaces.Services
{
    public interface IPermissionService : IBaseService
    {
        /// <summary>
        /// Check if a user has a specific permission
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="permissionName">The permission name</param>
        /// <returns>True if the user has the permission, otherwise false</returns>
        Task<bool> UserHasPermissionAsync(int userId, string permissionName);
        
        /// <summary>
        /// Get all permissions for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of permissions owned by the user</returns>
        Task<IEnumerable<PermissionEntity>> GetUserPermissionsAsync(int userId);
    }
}
