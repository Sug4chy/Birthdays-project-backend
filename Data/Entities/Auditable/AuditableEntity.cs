namespace Data.Entities.Auditable;

public abstract class AuditableEntity : IAuditableEntity
{
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
}