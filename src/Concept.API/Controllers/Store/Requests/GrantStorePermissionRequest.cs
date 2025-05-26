using FluentValidation;

namespace Concept.API.Controllers.Store.Requests
{
    /// <summary>
    /// 授予商店權限請求
    /// </summary>
    public class GrantStorePermissionRequest
    {
        /// <summary>
        /// 被授權使用者ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 權限過期時間 (可選)
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }
}
