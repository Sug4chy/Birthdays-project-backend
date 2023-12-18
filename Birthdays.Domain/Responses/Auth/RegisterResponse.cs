namespace Domain.Responses.Auth;

public record RegisterResponse : IResponse
{
    public required string Token { get; init; }
    public required Guid ProfileId { get; init; }
}