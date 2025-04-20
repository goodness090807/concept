using Concept.Core.Entities.ResourceAuthorization.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ResourceAccessAttribute : AuthorizeAttribute
    {
        public ResourceAccessAttribute(string resourceType, string resourceId, ResourcePermissionLevel resourcePermissionLevel)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
            ResourcePermissionLevel = resourcePermissionLevel;
            Policy = $"ResourceAccess:{resourceType}:{resourceId}:{resourcePermissionLevel}";
        }

        public string ResourceType { get; }
        public string ResourceId { get; }
        public ResourcePermissionLevel ResourcePermissionLevel { get; }
    }
}
