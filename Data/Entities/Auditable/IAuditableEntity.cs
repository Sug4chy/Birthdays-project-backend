namespace Data.Entities.Auditable;

public interface IAuditableEntity
{
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
}