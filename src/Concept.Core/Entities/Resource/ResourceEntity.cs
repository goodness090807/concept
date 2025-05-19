using Concept.Core.Entities.User;

namespace Concept.Core.Entities.Resource
{
    public class ResourceEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ResourceKey { get; set; } = string.Empty;

        public int ParentResourceId  { get; set; }

        /// <summary>
        /// 資源擁有者
        /// </summary>
        public int OwnerId { get; set; }

        public UserEntity Owner { get; set; } = null!;
    }
}
