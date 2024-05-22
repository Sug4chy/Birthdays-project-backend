using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Telegram;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService,
    IUserService userService,
    IProfileService profileService,
    RegisterRequestValidator requestValidator,
    ITelegramService telegramService,
    ILogger<RegisterHandler> logger)
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {request.Email}.");
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
            UnauthorizedException.ThrowByError(registerResult.Error);
        }

        var tokensModel = await authService.GenerateAndSetTokensAsync(user, ct);
        
        logger.LogInformation($"User with email {request.Email} was successfully registered.");
        return new RegisterResponse
        {
            AccessToken = tokensModel.AccessToken,
            RefreshToken = tokensModel.RefreshToken,
            TgBotLink = telegramService.GenerateLinkForUser(user)
        };
    }
}