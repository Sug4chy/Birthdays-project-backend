namespace Domain.DTO.Responses.Auth;

public record RegisterResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required string TgBotLink { get; init; }
}