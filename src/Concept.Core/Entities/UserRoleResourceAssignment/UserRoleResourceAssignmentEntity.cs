namespace Concept.Core.Entities.UserRoleResourceAssignment
{
    public class UserRoleResourceAssignmentEntity : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int ResourceId { get; set; }
    }
}