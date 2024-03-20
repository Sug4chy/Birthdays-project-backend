using System.ComponentModel.DataAnnotations;
using Data.Entities.Auditable;

namespace Data.Entities;

public class Wish : AuditableEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(int.MaxValue)]
    public required string Name { get; set; }
    public required Guid WishListId { get; set; }
    public WishList? WishList { get; set; }
    public Guid? ImageId { get; set; }
    public DatabaseFile? Image { get;set; }
    
    [MaxLength(int.MaxValue)]
    public string? GiftRef { get; set; }
    
    [MaxLength(int.MaxValue)]
    public string? Description { get; set; }
}