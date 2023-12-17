using Web.Models;
using Web.Services.Auth;
using Web.Services.Profiles;
using Web.Services.Users;

namespace Web.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService, 
    IUserService userService, 
    IProfileService profileService
    )
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        var profile = await profileService.CreateAsync(ct);
        var user = await userService.CreateUserAsync(request, profile, ct);
        await authService.RegisterUserAsync(new RegisterModel
        {
            User = user, 
            Password = await authService.HashPassword(request.Password, ct)
        }, ct);
        await profileService.CommitAsync();

        return new RegisterResponse
        {
            ProfileId = profile.Id, 
            Token = await authService.GenerateToken(user, ct)
        };
    }
}

public record RegisterRequest
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }
    public DateTime BirthDate { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record RegisterResponse
{
    public required string Token { get; init; }
    public required Guid ProfileId { get; init; }
}