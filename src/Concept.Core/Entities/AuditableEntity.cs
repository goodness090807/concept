namespace Concept.Core.Entities
{
    public class AuditableEntity : TimestampedEntity, IAuditableEntity
    {
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }
    }
}
