namespace Domain.DTO.Responses.Profiles;

public record GetProfilesByPageIndexResponse : IResponse
{
    public required MainPageProfileDto[] Profiles { get; init; }
}