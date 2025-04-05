namespace Concept.Core.Entities
{
    public abstract class TimestampedEntity : ITimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
