using Data.Entities;

namespace Web.Models;

public record RegisterModel
{
    public required User User { get; init; }
    public required string Password { get; init; }
}