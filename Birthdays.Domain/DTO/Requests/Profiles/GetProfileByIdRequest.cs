namespace Domain.DTO.Requests.Profiles;

public record GetProfileByIdRequest
{
    public required Guid ProfileId { get; init; }
}