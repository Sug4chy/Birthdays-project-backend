namespace Domain.DTO.Responses.Profiles;

public record GetCurrentProfileResponse
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Patronymic { get; init; }
    public required DateDto Birthdate { get; init; }
    public required ProfileDto Profile { get; init; }
}