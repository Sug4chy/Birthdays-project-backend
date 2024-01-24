namespace Domain.Models;

public record AuthTokensModel
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}