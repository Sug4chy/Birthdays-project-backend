namespace Domain.DTO.Requests.Profiles;

public record GetCurrentProfileRequest
{
    public required string Jwt { get; init; }
}