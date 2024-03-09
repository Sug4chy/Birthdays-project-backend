namespace Domain.DTO.Responses.Auth;

public record LoginResponse : IResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}