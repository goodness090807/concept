using Concept.Core.Entities.ResourceAuthorization.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.ResourceAccess
{
    public class ResourceAccessPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string DefaultPolicyName = "DefaultPolicy";
        private const string POLICY_PREFIX = "ResourceAccess";

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            // 定義預設策略，例如需要已驗證的使用者
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            return Task.FromResult(defaultPolicy);
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            Task.FromResult<AuthorizationPolicy?>(null);

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // 根據策略名稱返回對應的策略
            if (policyName == DefaultPolicyName)
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            // 針對自訂策略名稱進行處理
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                // 策略名稱格式為 "ResourceAccess:{ResourceType}:{ResourceId}:{PermissionLevel}"
                var parts = policyName.Split(':');
                if (parts.Length == 4 && Enum.TryParse(parts[3], true, out ResourcePermissionLevel permissionLevel))
                {
                    var resourceType = parts[1];
                    var resourceId = parts[2];

                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new ResourceAccessRequirement(resourceType, resourceId, permissionLevel))
                        .Build();

                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
            }

            // 如果找不到對應的策略，返回 null
            return Task.FromResult<AuthorizationPolicy?>(null);
        }
    }
}
