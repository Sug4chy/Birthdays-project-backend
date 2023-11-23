namespace Data.Entities;

public class Profile : IAuditableEntity
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public User? User { get; set; }
    public IEnumerable<Subscription>? Subscriptions { get; set; }
    public IEnumerable<Chat>? Chats { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}