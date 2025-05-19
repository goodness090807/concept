using Concept.API.Extensions;
using Concept.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.PermissionAccess
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PermissionHandler> _logger;

        public PermissionHandler(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor, ILogger<PermissionHandler> logger)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogWarning("無法獲取 HttpContext");
                return;
            }

            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                _logger.LogWarning("使用者未通過身份驗證");
                return;
            }

            var userId = context.User.GetUserId();
            if (userId == 0)
            {
                _logger.LogWarning("無法獲取用戶ID");
                return;
            }

            bool hasPermission = await _permissionService.UserHasPermissionAsync(userId, requirement.PermissionName);

            if (hasPermission)
            {
                context.Succeed(requirement);
                _logger.LogInformation("授權成功：用戶 {UserId} 具有權限 {Permission}", userId, requirement.PermissionName);
            }
            else
            {
                _logger.LogWarning("授權失敗：用戶 {UserId} 不具有權限 {Permission}", userId, requirement.PermissionName);
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync($"Forbidden: 無權限執行此操作，需要 {requirement.PermissionName} 權限");
            }
        }
    }
}
