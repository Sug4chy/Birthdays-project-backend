namespace Domain.DTO;

public record UserDto
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required ProfileDto Profile { get; init; }
}