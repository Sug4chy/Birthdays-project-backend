using Data.Entities.Auditable;

namespace Data.Entities;

public class Wish : AuditableEntity
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
    public required Guid WishListId { get; set; }
    public WishList? WishList { get; set; }
    public Guid? ImageId { get; set; }
    public DatabaseFile? Image { get;set; }
    public string? GiftRef { get; set; }
    public string? Description { get; set; }
}