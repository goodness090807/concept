namespace Concept.Core.Interfaces.Services
{
    public interface IRolePermissionService : IBaseService
    {
        /// <summary>
        /// 檢查使用者是否擁有特定角色
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="roleName">角色名稱</param>
        /// <returns>是否有權限</returns>
        Task<bool> HasRoleAsync(int userId, string roleName);

        /// <summary>
        /// 檢查使用者是否擁有特定資源的角色
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="roleName">角色名稱</param>
        /// <param name="resourceType">資源類型</param>
        /// <param name="resourceId">資源ID</param>
        /// <returns>是否有權限</returns>
        Task<bool> HasRoleForResourceAsync(int userId, string roleName, string resourceType, int resourceId);
    }
}
