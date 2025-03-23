namespace Concept.Core.Entities.User.Enums
{
    public enum UserStatus
    {
        /// <summary>
        /// 已啟動
        /// </summary>
        Active = 1,
        /// <summary>
        /// 待驗證
        /// </summary>
        Inactive = 2,
        /// <summary>
        /// 已封鎖
        /// </summary>
        Blocked = 3,
        /// <summary>
        /// 已刪除
        /// </summary>
        Deleted = 4
    }
}
