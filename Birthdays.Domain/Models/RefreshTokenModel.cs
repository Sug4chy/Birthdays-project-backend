namespace Domain.Models;

public record RefreshTokenModel
{
    public required string Token { get; init; }
    public required DateTime TokenExpiryTime { get; init; }
}