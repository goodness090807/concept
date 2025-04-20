using Concept.Core.Entities.ResourceAuthorization.Enums;

namespace Concept.Core.Interfaces.Services
{
    public interface IResourcePermissionService : IBaseService
    {
        /// <summary>
        /// 檢查使用者是否擁有資源的權限
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="userId"></param>
        /// <param name="resourcePermissionLevel"></param>
        /// <param name="checkInactiveResources"></param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(string resourceType, string resourceKey, int userId, ResourcePermissionLevel resourcePermissionLevel, bool checkInactiveResources = false);
    }
}