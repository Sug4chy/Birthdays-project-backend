namespace Domain.DTO.Requests.Auth;

public record LogoutRequest
{
    public required string Email { get; init; }
}