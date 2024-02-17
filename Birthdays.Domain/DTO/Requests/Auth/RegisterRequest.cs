namespace Domain.DTO.Requests.Auth;

public record RegisterRequest
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }
    public DateOnly BirthDate { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}