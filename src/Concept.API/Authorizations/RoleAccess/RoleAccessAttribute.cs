using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.RoleAccess
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoleAccessAttribute : AuthorizeAttribute
    {
        public RoleAccessAttribute(string requiredRole, string resourceType = "", string resourceIdParameter = "")
        {
            RequiredRole = requiredRole;
            ResourceType = resourceType;
            ResourceIdParameter = resourceIdParameter;
            Policy = $"RoleAccess:{requiredRole}:{resourceType}:{resourceIdParameter}";
        }

        public string RequiredRole { get; }
        public string ResourceType { get; }
        public string ResourceIdParameter { get; }
    }
}
