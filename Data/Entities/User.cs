using Data.Entities.Auditable;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class User : IdentityUser, IAuditableEntity
{
    public required Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? Patronymic { get; set; }
    public required DateOnly BirthDate { get; set; }
    public string? CurrentRefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
}