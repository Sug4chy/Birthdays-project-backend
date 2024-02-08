﻿using AutoMapper;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Users;
using Domain.Validators;
using Domain.Validators.Auth;
using Serilog;

namespace Domain.Handlers.Auth;

public class LoginHandler(
    LoginRequestValidator validator,
    IAuthService authService,
    IMapper mapper,
    IUserService userService)
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken ct = default)
    {
        Log.Information($"Login request from user {request.Email} was received.");
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsSuccess)
        {
            throw new CustomValidationException(validationResult.Error);
        }

        var loginModel = mapper.Map<LoginModel>(request);
        var authResult = await authService.LoginUserAsync(loginModel, ct);
        if (!authResult.IsSuccess)
        {
            Log.Error($"\"{authResult.Error.Description}\" error was occurred while login " +
                      $"user with email {request.Email}");
            IdentityException.ThrowByError(authResult.Error);
        }

        var user = await userService.GetUserByEmailAsync(request.Email, ct);
        NotFoundException.ThrowIfNull(user, $"User with email {request.Email} wasn't found");
        
        var tokensModel = await authService.GenerateAndSetTokensAsync(user!, ct);
        
        Log.Information($"Login response was successfully sent for user {request.Email}");
        return new LoginResponse
        {
            AccessToken = tokensModel.AccessToken,
            RefreshToken = tokensModel.RefreshToken
        };
    }
}