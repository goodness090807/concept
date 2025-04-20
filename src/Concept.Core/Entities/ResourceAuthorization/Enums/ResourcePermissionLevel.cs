namespace Concept.Core.Entities.ResourceAuthorization.Enums
{
    public enum ResourcePermissionLevel
    {
        /// <summary>
        /// 可查看資源
        /// </summary>
        VIEWER = 1,
        /// <summary>
        /// 可編輯資源
        /// </summary>
        EDITOR = 2,
        /// <summary>
        /// 可管理資源
        /// </summary>
        ADMIN = 500,
        /// <summary>
        /// 資源擁有者
        /// </summary>
        OWNER = 999
    }
}
