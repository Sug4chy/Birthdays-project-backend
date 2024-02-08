namespace Domain.DTO.Responses.Profiles;

public record GetProfileByIdResponse : IResponse
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Patronymic { get; init; }
    public required DateOnly Birthdate { get; init; }
    public required ProfileDto Profile { get; init; }
}