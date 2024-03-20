using System.ComponentModel.DataAnnotations;
using Data.Entities.Auditable;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class User : IdentityUser, IAuditableEntity
{
    public required Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    
    [MaxLength(50)]
    public required string Name { get; set; }
    
    [MaxLength(50)]
    public required string Surname { get; set; }
    
    [MaxLength(50)]
    public string? Patronymic { get; set; }
    public required DateOnly BirthDate { get; set; }
    
    [MaxLength(50)]
    public string? CurrentRefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
}