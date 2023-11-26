using Data.Entities.Auditable;

namespace Data.Entities;

public class Subscription : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required Guid SubscriberId { get; set; }
    public Profile? Subscriber { get; set; }
}