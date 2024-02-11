namespace Domain.DTO.Requests.Profiles;

public record GetProfileByUsernameRequest
{
    public required string Username { get; init; }
    public required string Jwt { get; init; }
}