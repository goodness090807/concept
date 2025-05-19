namespace Concept.Core.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        /// <summary>
        /// 檢查使用者是否擁有指定角色
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="roleName">角色名稱</param>
        /// <returns>是否有角色</returns>
        Task<bool> UserHasRoleAsync(int userId, string roleName);

        /// <summary>
        /// 檢查使用者是否擁有指定資源的指定角色
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="roleName">角色名稱</param>
        /// <param name="resourceType">資源類型</param>
        /// <param name="resourceId">資源ID</param>
        /// <returns>是否有角色</returns>
        Task<bool> UserHasRoleForResourceAsync(int userId, string roleName, string resourceType, int resourceId);
    }
}
