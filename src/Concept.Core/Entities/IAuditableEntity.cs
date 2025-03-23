namespace Concept.Core.Entities
{
    public interface IAuditableEntity
    {
        int? CreatedById { get; set; }
        int? UpdatedById { get; set; }
    }
}
