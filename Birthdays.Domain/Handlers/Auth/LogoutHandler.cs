using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Users;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class LogoutHandler(
    IAuthService authService, 
    IUserService userService,
    ILogger<LogoutHandler> logger)
{
    public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken ct = default)
    {
        logger.LogInformation($"Logout request was received for user {request.Email}");
        var user = await userService.GetUserByEmailAsync(request.Email, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithEmail(request.Email));
        
        var logoutResult = await authService.LogoutUserAsync(user!, ct);
        if (!logoutResult.IsSuccess)
        {
            UnauthorizedException.ThrowByError(logoutResult.Error);
        }
        
        logger.LogInformation($"Logout response was successfully sent for user {request.Email}");
        return new LogoutResponse();
    }
}