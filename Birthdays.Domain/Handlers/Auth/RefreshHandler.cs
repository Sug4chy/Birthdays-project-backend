using System.Security.Claims;
using Data.Context;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Validators;
using Microsoft.Extensions.Configuration;

namespace Domain.Handlers.Auth;

public class RefreshHandler(
    RefreshRequestValidator validator,
    ITokenService tokenService,
    IUserService userService,
    AppDbContext context,
    IConfiguration config)
{
    public async Task<RefreshResponse> Handle(RefreshRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsSuccess)
        {
            throw new CustomValidationException(validationResult.Error);
        }

        var principal = await tokenService
            .GetPrincipalFromExpiredTokenAsync(request.ExpiredAccessToken, ct);
        string? username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        NotFoundException.ThrowIfNull(username, "\"Username\" claim is required");

        var user = await userService.GetUserByEmailAsync(username!, ct);
        NotFoundException.ThrowIfNull(user, $"User with email {username} wasn't found");

        if (user!.CurrentRefreshToken != request.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new CustomValidationException(AuthErrors.InvalidRefreshToken);
        }

        string newAccessToken = await tokenService.GenerateAccessToken(user, ct);
        string newRefreshToken = await tokenService.GenerateRefreshToken(ct);

        user.CurrentRefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow
            .AddDays(config.GetValue<int>("RefreshTokenExpiresTime"));

        await context.SaveChangesAsync(ct);
        return new RefreshResponse
        {
            RefreshToken = newRefreshToken,
            AccessToken = newAccessToken
        };
    }
}