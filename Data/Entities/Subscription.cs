namespace Birthdays.Data.Entities;

public class Subscription : IAuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required Guid SubscriberId { get; set; }
    public Profile? Subscriber { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}