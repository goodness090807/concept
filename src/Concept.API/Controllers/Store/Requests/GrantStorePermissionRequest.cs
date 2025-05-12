using Concept.Core.Entities.ResourceAuthorization.Enums;
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
        /// 權限等級
        /// </summary>
        public ResourcePermissionLevel PermissionLevel { get; set; }

        /// <summary>
        /// 權限過期時間 (可選)
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
    }

    /// <summary>
    /// 授予商店權限請求驗證器
    /// </summary>
    public class GrantStorePermissionRequestValidator : AbstractValidator<GrantStorePermissionRequest>
    {
        public GrantStorePermissionRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("使用者ID必須大於0");
            RuleFor(x => x.PermissionLevel).IsInEnum().WithMessage("權限等級無效");
            RuleFor(x => x.PermissionLevel).NotEqual(ResourcePermissionLevel.OWNER)
                .WithMessage("不能授予所有者權限，所有者權限僅限於創建時分配");
            RuleFor(x => x.ExpiresAt).Must(x => x == null || x > DateTime.UtcNow)
                .WithMessage("過期時間必須是未來的時間");
        }
    }
}
