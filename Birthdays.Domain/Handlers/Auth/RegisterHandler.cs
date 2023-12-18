using Domain.Models;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Users;

namespace Domain.Handlers.Auth;

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
        await profileService.CommitAsync(ct);

        return new RegisterResponse
        {
            ProfileId = profile.Id, 
            Token = await authService.GenerateToken(user, ct)
        };
    }
}