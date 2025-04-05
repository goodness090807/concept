using Concept.Core.Entities.User;

namespace Concept.Core.Entities.Store
{
    public class StoreEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
