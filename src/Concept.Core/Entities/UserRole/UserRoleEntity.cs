using Concept.Core.Entities.Role;
using Concept.Core.Entities.User;

namespace Concept.Core.Entities.UserRoleEntity
{
    public class UserRoleEntity : AuditableEntity
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public UserEntity User { get; set; } = new UserEntity();

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = new RoleEntity();
    }
}