using Concept.Core.Entities.ResourceAuthorization.Enums;

namespace Concept.Core.Interfaces.Repositories
{
    public interface IResourceRepository
    {
        /// <summary>
        /// 新增資源，並且設定擁有者
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<int> AddResourceAsync(string name, string resourceType, string resourceKey, int ownerId);

        /// <summary>
        /// 給予資源權限
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="userId"></param>
        /// <param name="grantedByUserId"></param>
        /// <param name="permissionLevel"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        Task<int> GrantResourceAccessAsync(int resourceId, int userId, int grantedByUserId, ResourcePermissionLevel permissionLevel, DateTime? expiresAt);

        /// <summary>
        /// 取得資源權限
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="userId"></param>
        /// <param name="permissionLevel"></param>
        /// <param name="checkInactiveResources"></param>
        /// <returns></returns>
        Task<bool> GetPermissionAsync(string resourceType, string resourceKey, int userId, ResourcePermissionLevel permissionLevel, bool checkInactiveResources = false);

        /// <summary>
        /// 取得使用者對指定類型資源的所有權限
        /// </summary>
        /// <param name="resourceType">資源類型</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>資源權限清單</returns>
        Task<List<(int ResourceId, string ResourceKey, string ResourceName, int OwnerId, DateTimeOffset? ExpiresAt, ResourcePermissionLevel PermissionLevel)>> 
            GetUserResourcePermissionsAsync(string resourceType, int userId, bool includeExpired = false);
    }
}
