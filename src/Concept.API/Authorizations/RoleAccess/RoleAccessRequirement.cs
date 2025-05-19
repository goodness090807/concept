using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.RoleAccess
{
    public class RoleAccessRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }
        public string ResourceType { get; }
        public string ResourceIdParameter { get; }

        public RoleAccessRequirement(string requiredRole, string resourceType = "", string resourceIdParameter = "")
        {
            RequiredRole = requiredRole;
            ResourceType = resourceType;
            ResourceIdParameter = resourceIdParameter;
        }
    }
}
