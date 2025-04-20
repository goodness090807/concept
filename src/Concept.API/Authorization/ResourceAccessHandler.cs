using Concept.API.Extensions;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorization
{
    public class ResourceAccessHandler : AuthorizationHandler<ResourceAccessRequirement>
    {
        private readonly IResourcePermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ResourceAccessHandler> _logger;

        public ResourceAccessHandler(IResourcePermissionService permissionService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ResourceAccessHandler> logger)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceAccessRequirement requirement) 
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("無法獲取 HttpContext");
                return;
            }

            // 檢查使用者是否已通過身份驗證
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                _logger.LogWarning("使用者未通過身份驗證");
                return; // 讓 [Authorize] 處理未驗證的情況
            }

            var userId = context.User.GetUserId();
            if (userId == 0)
            {
                _logger.LogWarning("無法獲取用戶ID");
                return;
            }

            var resourceId = httpContext.Request.RouteValues[requirement.ResourceIdParameter] as string ?? string.Empty;
            if (resourceId == "")
            {
                _logger.LogWarning("無法獲取資源ID: {ResourceIdParam}", requirement.ResourceIdParameter);
                return;
            }

            var resourceType = requirement.ResourceType;

            var hasAccess = await _permissionService.HasPermissionAsync(resourceType, resourceId, userId, requirement.ResourcePermissionLevel);
            if (hasAccess)
            {
                context.Succeed(requirement);
                _logger.LogInformation("授權成功");
            }
            else
            {
                _logger.LogWarning("用戶 {UserId} 無權操作資源類型 {ResourceType} 的資源 {ResourceId}", userId, resourceType, resourceId);
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Forbidden: 無權限操作此資源");
            }
        }
    }
}