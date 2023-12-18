namespace Domain.Responses.Auth;

public record RegisterResponse
{
    public required string Token { get; init; }
    public required Guid ProfileId { get; init; }
}