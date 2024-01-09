namespace Domain.DTO.Responses.Auth;

public record RegisterResponse : IResponse
{
    public required string Token { get; init; }
    public required UserDto User { get; init; }
}