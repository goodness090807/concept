namespace Concept.Core.Entities
{
    public interface ITimestampedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}
