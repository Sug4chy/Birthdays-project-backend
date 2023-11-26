using Data.Entities.Auditable;

namespace Data.Entities;

public class WishList : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Wish>? Wishes { get; set; }
}