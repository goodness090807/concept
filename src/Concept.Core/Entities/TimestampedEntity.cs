namespace Concept.Core.Entities
{
    public abstract class TimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
