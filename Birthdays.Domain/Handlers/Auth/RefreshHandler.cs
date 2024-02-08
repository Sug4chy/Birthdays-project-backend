﻿using System.Security.Claims;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Validators;
using Domain.Validators.Auth;

namespace Domain.Handlers.Auth;

public class RefreshHandler(
    RefreshRequestValidator validator,
    ITokenService tokenService,
    IUserService userService,
    IAuthService authService)
{
    public async Task<RefreshResponse> Handle(RefreshRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsSuccess)
        {
            throw new CustomValidationException(validationResult.Error);
        }

        var principal = tokenService
            .GetPrincipalFromExpiredTokenAsync(request.ExpiredAccessToken);
        string? username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        NotFoundException.ThrowIfNull(username, "\"Username\" claim is required");

        var user = await userService.GetUserByEmailAsync(username!, ct);
        NotFoundException.ThrowIfNull(user, $"User with email {username} wasn't found");

        if (user!.CurrentRefreshToken != request.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new CustomValidationException(AuthErrors.InvalidRefreshToken);
        }
        
        var tokensModel = await authService.GenerateAndSetTokensAsync(user, ct);
        return new RefreshResponse
        {
            RefreshToken = tokensModel.RefreshToken,
            AccessToken = tokensModel.AccessToken
        };
    }
}