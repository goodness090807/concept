using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Concept.API.Authorizations.StorePermission
{
    public class StorePermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public StorePermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _options = options.Value;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicy(_options.DefaultPolicy.Requirements, _options.DefaultPolicy.AuthenticationSchemes));
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("StorePermission:"))
            {
                var permissionName = policyName.Substring("StorePermission:".Length);
                return Task.FromResult<AuthorizationPolicy?>(new AuthorizationPolicy(new[] { new StorePermissionRequirement(permissionName) }, Array.Empty<string>()));
            }

            return Task.FromResult<AuthorizationPolicy?>(null);
        }
    }
}