using Concept.Core.Entities.Role;
using Concept.Core.Entities.Store;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.UserStoreRole
{
    public class UserStoreRoleEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public int StoreId { get; set; }
        public StoreEntity Store { get; set; } = null!;
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}