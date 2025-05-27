using Concept.Core.Entities.Permission;
using Concept.Core.Entities.Store;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.UserStorePermission
{
    public class UserStorePermissionEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public int StoreId { get; set; }
        public StoreEntity Store { get; set; } = null!;
        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}