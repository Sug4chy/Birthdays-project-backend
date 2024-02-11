﻿using System.Security.Claims;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class RefreshHandler(
    RefreshRequestValidator validator,
    ITokenService tokenService,
    IUserService userService,
    IAuthService authService,
    ILogger<RefreshHandler> logger)
{
    public async Task<RefreshResponse> Handle(RefreshRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Refresh request was received from user " +
                              $"with refresh token {request.RefreshToken}");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var principal = tokenService
            .GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
        string? username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        NotFoundException.ThrowIfNull(username, AuthErrors.DoesNotIncludeClaim(ClaimTypes.Email));

        var user = await userService.GetUserByEmailAsync(username!, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithEmail(username!));

        if (user!.CurrentRefreshToken != request.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new ForbiddenException
            {
                Error = AuthErrors.InvalidRefreshToken
            };
        }

        var tokensModel = await authService.GenerateAndSetTokensAsync(user, ct);
        logger.LogInformation($"Refresh response was successfully sent to user with email {user.Email}");
        return new RefreshResponse
        {
            RefreshToken = tokensModel.RefreshToken,
            AccessToken = tokensModel.AccessToken
        };
    }
}