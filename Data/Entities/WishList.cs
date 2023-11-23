namespace Data.Entities;

public class WishList : IAuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<Wish>? Wishes { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}