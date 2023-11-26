using Microsoft.AspNetCore.Identity;

namespace Birthdays.Data.Entities;

public class User : IdentityUser, IAuditableEntity
{
    public required Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string? Patronymic { get; set; }
    public required DateOnly BirthDate { get; set; }
    public DateTime CreatingTime { get; set; }
    public DateTime EditingTime { get; set; }
    public DateTime? DeletingTime { get; set; } = null;
}