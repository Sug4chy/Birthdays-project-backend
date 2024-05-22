namespace Domain.DTO.Responses.Auth;

public record RefreshResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}