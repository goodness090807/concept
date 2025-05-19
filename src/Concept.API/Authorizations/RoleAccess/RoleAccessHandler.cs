using Concept.API.Extensions;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.RoleAccess
{
    public class RoleAccessHandler : AuthorizationHandler<RoleAccessRequirement>
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RoleAccessHandler> _logger;

        public RoleAccessHandler(
            IRolePermissionService rolePermissionService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RoleAccessHandler> logger)
        {
            _rolePermissionService = rolePermissionService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleAccessRequirement requirement)
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

            bool hasAccess;
            
            // 檢查是否需要針對特定資源進行權限驗證
            if (!string.IsNullOrEmpty(requirement.ResourceType) && !string.IsNullOrEmpty(requirement.ResourceIdParameter))
            {
                var resourceId = httpContext.Request.RouteValues[requirement.ResourceIdParameter] as string ?? string.Empty;
                if (resourceId == "")
                {
                    _logger.LogWarning("無法獲取資源ID: {ResourceIdParam}", requirement.ResourceIdParameter);
                    return;
                }

                int resourceIdValue = int.Parse(resourceId);
                hasAccess = await _rolePermissionService.HasRoleForResourceAsync(
                    userId, 
                    requirement.RequiredRole, 
                    requirement.ResourceType, 
                    resourceIdValue
                );
            }
            else
            {
                // 純角色權限檢查
                hasAccess = await _rolePermissionService.HasRoleAsync(userId, requirement.RequiredRole);
            }

            if (hasAccess)
            {
                context.Succeed(requirement);
                _logger.LogInformation("授權成功");
            }
            else
            {
                if (!string.IsNullOrEmpty(requirement.ResourceType) && !string.IsNullOrEmpty(requirement.ResourceIdParameter))
                {
                    var resourceId = httpContext.Request.RouteValues[requirement.ResourceIdParameter]?.ToString() ?? "未知";
                    _logger.LogWarning("用戶 {UserId} 沒有角色 {Role} 來操作資源類型 {ResourceType} 的資源 {ResourceId}",
                        userId, requirement.RequiredRole, requirement.ResourceType, resourceId);
                }
                else
                {
                    _logger.LogWarning("用戶 {UserId} 沒有角色 {Role}", userId, requirement.RequiredRole);
                }

                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Forbidden: 無權限操作此資源");
            }
        }
    }
}
