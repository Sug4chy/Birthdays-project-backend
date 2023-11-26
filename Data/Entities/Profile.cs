using Data.Entities.Auditable;

namespace Data.Entities;

public class Profile
    : AuditableEntity
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public User? User { get; set; }
    public ICollection<Subscription>? SubscriptionsAsBirthdayMan { get; set; }
    public ICollection<Subscription>? SubscriptionsAsSubscriber { get; set; }
    public ICollection<Chat>? Chats { get; set; }
    public ICollection<WishList>? WishLists { get; set; }
}