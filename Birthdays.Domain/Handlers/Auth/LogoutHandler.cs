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
        
        var logoutResult = await authService.LogoutUserAsync(user!, ct);
        if (!logoutResult.IsSuccess)
        {
            Log.Error($"{logoutResult.Error.Description} error occurred while " +
                      $"logging out user with email {user!.Email}");
            UnauthorizedException.ThrowByError(logoutResult.Error);
        }
        
        Log.Information($"Logout response was successfully sent for user {request.Email}");
        return new LogoutResponse();
    }
}