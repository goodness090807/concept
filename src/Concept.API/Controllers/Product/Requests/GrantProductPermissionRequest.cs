using Concept.Core.Entities.ResourceAuthorization.Enums;
using FluentValidation;

namespace Concept.API.Controllers.Product.Requests
{
    /// <summary>
    /// 授予商品權限請求
    /// </summary>
    public class GrantProductPermissionRequest
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

    public class GrantProductPermissionRequestValidator : AbstractValidator<GrantProductPermissionRequest>
    {
        public GrantProductPermissionRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("使用者ID必須大於0");

            RuleFor(x => x.PermissionLevel)
                .Must(level => level != ResourcePermissionLevel.OWNER)
                .WithMessage("不能授予擁有者權限");

            RuleFor(x => x.ExpiresAt)
                .Must(date => date == null || date > DateTime.UtcNow)
                .WithMessage("過期時間必須晚於現在");
        }
    }
}
