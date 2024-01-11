using AutoMapper;
using Data.Context;
using Domain.DTO;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Validators;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Domain.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService,
    IUserService userService,
    IProfileService profileService,
    AppDbContext context,
    RegisterRequestValidator requestValidator,
    IMapper mapper,
    ITokenService tokenService,
    IConfiguration config
)
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        Log.Information($"Register request from user {request.Email} was received.");
        var validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsSuccess)
        {
            throw new CustomValidationException(validationResult.Error);
        }

        var profile = await profileService.CreateAsync(ct);
        var user = await userService.CreateUserAsync(request, profile, ct);

        var registerResult = await authService.RegisterUserAsync(new RegisterModel
        {
            User = user,
            Password = request.Password
        }, ct);

        if (!registerResult.IsSuccess)
        {
            Log.Error($"\"{registerResult.Error.Description}\" " +
                      $"error was occurred while registering user {request.Email}");
            IdentityException.ThrowByError(registerResult.Error);
        }

        string accessToken = await tokenService.GenerateAccessToken(user, ct);
        string refreshToken = await tokenService.GenerateRefreshToken(ct);

        user.CurrentRefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow
            .AddDays(config.GetValue<int>("RefreshTokenExpiresTime"));
        
        await context.SaveChangesAsync(ct);

        Log.Information($"Register response was successfully sent for user {request.Email}");
        return new RegisterResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = mapper.Map<UserDto>(user)
        };
    }
}