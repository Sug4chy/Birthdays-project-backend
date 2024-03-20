using System.ComponentModel.DataAnnotations;
using Data.Entities.Auditable;

namespace Data.Entities;

public class Chat : AuditableEntity
{
    public Guid Id { get; set; }
    public required Guid BirthdayManId { get; set; }
    public Profile? BirthdayMan { get; set; }
    public required ChatType ChatType { get; set; }
    
    [MaxLength(int.MaxValue)]
    public required string ChatUrl { get; set; }
}