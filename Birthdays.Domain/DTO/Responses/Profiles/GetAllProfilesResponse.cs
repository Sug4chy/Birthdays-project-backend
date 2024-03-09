namespace Domain.DTO.Responses.Profiles;

public record GetAllProfilesResponse : IResponse
{
    public required MainPageProfileDto[] Profiles { get; init; }
}