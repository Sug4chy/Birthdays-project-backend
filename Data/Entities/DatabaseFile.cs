using Data.Entities.Auditable;

namespace Data.Entities;

public class DatabaseFile : AuditableEntity
{
    public Guid Id { get; set; }
    public Wish? Wish { get; set; }
    public required string Name { get; set; }
    public required string ContentType { get; set; }
    public required string FileRef { get; set; }
}