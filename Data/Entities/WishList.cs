using System.ComponentModel.DataAnnotations;
using Data.Entities.Auditable;

namespace Data.Entities;

public class WishList : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    
    [MaxLength(int.MaxValue)]
    public required string Name { get; set; }
    
    [MaxLength(int.MaxValue)]
    public string? Description { get; set; }
    public ICollection<Wish>? Wishes { get; set; }
}