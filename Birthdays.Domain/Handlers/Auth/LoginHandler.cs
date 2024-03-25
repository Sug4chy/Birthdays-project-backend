using AutoMapper;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Telegram;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class LoginHandler(
    LoginRequestValidator validator,
    IAuthService authService,
    IMapper mapper,
    IUserService userService,
    ITelegramService telegramService,
    ILogger<LoginHandler> logger)
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken ct = default)
    {
        logger.LogInformation($"{request.GetType().Name} was received " +
                              $"from user with email {request.Email}.");
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var loginModel = mapper.Map<LoginModel>(request);
        var authResult = await authService.LoginUserAsync(loginModel, ct);
        if (!authResult.IsSuccess)
        {
            UnauthorizedException.ThrowByError(authResult.Error);
        }

        var user = await userService.GetUserByEmailAsync(request.Email, ct);
        NotFoundException.ThrowIfNull(user, UsersErrors.NoSuchUserWithEmail(request.Email));

        var tokensModel = await authService.GenerateAndSetTokensAsync(user!, ct);

        logger.LogInformation($"User with email {request.Email} was logged in.");
        return new LoginResponse
        {
            AccessToken = tokensModel.AccessToken,
            RefreshToken = tokensModel.RefreshToken,
            TgBotLink = telegramService.GenerateLinkForUser(user!)
        };
    }
}