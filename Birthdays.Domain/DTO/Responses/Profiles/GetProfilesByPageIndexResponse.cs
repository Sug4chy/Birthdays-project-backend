namespace Domain.DTO.Responses.Profiles;

public record GetProfilesByPageIndexResponse
{
    public required MainPageProfileDto[] Profiles { get; init; }
}