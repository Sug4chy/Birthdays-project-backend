namespace Domain.DTO.Requests.Auth;

public record LogoutRequest
{
    public required UserDto User { get; init; }
}