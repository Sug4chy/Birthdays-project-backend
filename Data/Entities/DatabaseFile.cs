namespace Data.Entities;

public class DatabaseFile : IAuditableEntity
{
    public Guid Id { get; set; }
    public Wish? Wish { get; set; }
    public required string Name { get; set; }
    public required string ContentType { get; set; }
    public required string FileRef { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}