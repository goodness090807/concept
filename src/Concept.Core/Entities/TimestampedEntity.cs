namespace Concept.Core.Entities
{
    public abstract class TimestampedEntity : ITimestampedEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
