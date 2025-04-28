using Concept.Core.Entities.ResourceAuthorization.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Concept.API.Authorizations.ResourceAccess
{
    public class ResourceAccessRequirement : IAuthorizationRequirement
    {
        public string ResourceType { get; }
        public ResourcePermissionLevel ResourcePermissionLevel { get; }
        public string ResourceIdParameter { get; }

        public ResourceAccessRequirement(string resourceType, string resourceIdParameter, ResourcePermissionLevel resourcePermissionLevel)
        {
            ResourceType = resourceType;
            ResourceIdParameter = resourceIdParameter;
            ResourcePermissionLevel = resourcePermissionLevel;
        }
    }
}