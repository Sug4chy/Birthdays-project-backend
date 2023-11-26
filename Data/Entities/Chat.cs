namespace Birthdays.Data.Entities;

public class Chat : IAuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required ChatType ChatType { get; set; }
    public required string ChatUrl { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}