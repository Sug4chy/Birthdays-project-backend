namespace Domain.DTO.Responses.Profiles;

public record MainPageProfileDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }
    public required DateDto BirthDate { get; init; }
}