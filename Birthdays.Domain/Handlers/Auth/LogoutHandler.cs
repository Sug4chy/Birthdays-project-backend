using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Services.Auth;
using Domain.Services.Users;
using Serilog;

namespace Domain.Handlers.Auth;

public class LogoutHandler(IAuthService authService, IUserService userService)
{
    public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken ct = default)
    {
        Log.Information($"Logout request was received for user {request.Email}");
        var user = await userService.GetUserByEmailAsync(request.Email, ct);
        NotFoundException.ThrowIfNull(user, $"User with email {request.Email} wasn't found");
        
        var possibleError = await authService.LogoutUserAsync(user!, ct);
        if (possibleError is not null)
        {
            Log.Error(possibleError.Message);
            throw new IdentityException
            {
                Errors = new[] { possibleError }
            };
        }
        
        Log.Information($"Logout response was successfully sent for user {request.Email}");
        return new LogoutResponse();
    }
}