using System.ComponentModel.DataAnnotations;
using Data.Entities.Auditable;

namespace Data.Entities;

public class DatabaseFile : AuditableEntity
{
    public Guid Id { get; set; }
    public Wish? Wish { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; }
    
    [MaxLength(20)]
    public required string ContentType { get; set; }
    
    [MaxLength(30)]
    public required string FileRef { get; set; }
}