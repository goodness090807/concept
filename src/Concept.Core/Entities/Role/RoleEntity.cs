using Concept.Core.Entities.RolePermission;

namespace Concept.Core.Entities.Role
{
    public class RoleEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<RolePermissionEntity> RolePermissions { get; set; } = new List<RolePermissionEntity>();
    }
}