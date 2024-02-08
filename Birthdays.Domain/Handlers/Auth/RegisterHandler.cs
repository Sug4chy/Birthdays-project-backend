using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Serilog;

namespace Domain.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService,
    IUserService userService,
    IProfileService profileService,
    RegisterRequestValidator requestValidator)
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        Log.Information($"Register request from user {request.Email} was received.");
        var validationResult = await requestValidator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

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
            UnauthorizedException.ThrowByError(registerResult.Error);
        }

        var tokensModel = await authService.GenerateAndSetTokensAsync(user, ct);
        
        Log.Information($"Register response was successfully sent for user {request.Email}");
        return new RegisterResponse
        {
            AccessToken = tokensModel.AccessToken,
            RefreshToken = tokensModel.RefreshToken
        };
    }
}