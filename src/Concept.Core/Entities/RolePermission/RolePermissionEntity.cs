using Concept.Core.Entities.Permission;
using Concept.Core.Entities.Role;

namespace Concept.Core.Entities.RolePermission
{
    public class RolePermissionEntity : AuditableEntity
    {
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; } = null!;
    }
}