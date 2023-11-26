namespace Birthdays.Data.Entities;

public interface IAuditableEntity
{
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}