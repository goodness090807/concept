using Concept.Core.Entities.Permission;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.UserPermission
{
    public class UserPermissionEntity : AuditableEntity
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public UserEntity User { get; set; } = new UserEntity();

        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; } = null!;
    }
}
