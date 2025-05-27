using Concept.API.Extensions;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.StorePermission
{
    public class StorePermissionHandler : AuthorizationHandler<StorePermissionRequirement>
    {
        private readonly string _storeIdParameter = "storeId";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<StorePermissionHandler> _logger;
        private readonly IUserService _userService;

        public StorePermissionHandler(IHttpContextAccessor httpContextAccessor, ILogger<StorePermissionHandler> logger, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger;
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StorePermissionRequirement requirement)
        {
            // TODO：Response格式化處理
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("無法獲取 HttpContext");
                return;
            }

            // 檢查使用者是否已通過身份驗證
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                // 讓 [Authorize] 處理未驗證的情況
                _logger.LogWarning("使用者未通過身份驗證");
                return;
            }

            var userId = context.User.GetUserId();
            if (userId == 0)
            {
                _logger.LogWarning("無法獲取用戶ID");
                return;
            }

            var storeId = httpContext.Request.RouteValues[_storeIdParameter] as string ?? string.Empty;
            if (storeId == "")
            {
                _logger.LogWarning("這個API沒有{StoreId}，請確認是否要使用這個授權", _storeIdParameter);
                return;
            }

            var result = await _userService.HasStorePermissionAsync(userId, int.Parse(storeId), requirement.PermissionName);

            if (result.IsFailure)
            {
                _logger.LogWarning("檢查商店權限失敗: {ErrorCode} - {ErrorMessage}", result.ErrorCode, result.Error);
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Forbidden: 無權限操作此資源");
                return;
            }

            if (result.Data)
            {
                context.Succeed(requirement);
                _logger.LogInformation("授權成功");
            }
            else
            {
                _logger.LogWarning("用戶 {UserId} 無權操作商店 {StoreId} 的資源", userId, storeId);
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Forbidden: 無權限操作此資源");
            }
        }
    }
}