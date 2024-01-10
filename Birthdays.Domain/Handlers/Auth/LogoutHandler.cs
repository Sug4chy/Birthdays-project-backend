using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Services.Auth;
using Serilog;

namespace Domain.Handlers.Auth;

public class LogoutHandler(IAuthService authService)
{
    public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken ct = default)
    {
        Log.Information($"Logout request was received for user {request.User}");
        await authService.LogoutUserAsync(ct);
        
        Log.Information($"Logout response was successfully sent for user {request.User}");
        return new LogoutResponse();
    }
}