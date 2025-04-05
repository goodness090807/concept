using Concept.Core.Entities.ResourceAuthorization;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.Resource
{
    public class ResourceEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceKey { get; set; } = string.Empty;

        /// <summary>
        /// 資源擁有者
        /// </summary>
        public int OwnerId { get; set; }

        public UserEntity Owner { get; set; } = null!;

        public ICollection<ResourceAuthorizationEntity> Authorizations { get; set; } = new List<ResourceAuthorizationEntity>();
    }
}
