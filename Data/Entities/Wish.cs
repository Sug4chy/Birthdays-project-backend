namespace Data.Entities;

public class Wish : IAuditableEntity
{
    public Guid Id { get; set; }
    public required Guid WishListId { get; set; }
    public WishList? WishList { get; set; }
    public Guid ImageId { get; set; }
    public DatabaseFile? Image { get;set; }
    public string? GiftRef { get; set; }
    public string? Description { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; }
}