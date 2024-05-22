using Domain.Accessors;
using Domain.Exceptions;
using Domain.Services.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class LogoutHandler(
    IAuthService authService,
    ICurrentUserAccessor userAccessor,
    ILogger<LogoutHandler> logger)
{
    public async Task Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);
        logger.LogInformation($"LogoutRequest was received " +
                              $"from user with email {currentUser.Email}.");
        
        var logoutResult = await authService.LogoutUserAsync(currentUser, ct);
        if (!logoutResult.IsSuccess)
        {
            UnauthorizedException.ThrowByError(logoutResult.Error);
        }
        
        logger.LogInformation($"User with email {currentUser.Email} was logged out.");
    }
}