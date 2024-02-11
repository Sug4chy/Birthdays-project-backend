using AutoMapper;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Results;
using Domain.Services.Auth;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Microsoft.Extensions.Logging;

namespace Domain.Handlers.Auth;

public class LoginHandler(
    LoginRequestValidator validator,
    IAuthService authService,
    IMapper mapper,
    IUserService userService,
    ILogger<LoginHandler> logger)
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken ct = default)
    {
        logger.LogInformation($"Login request from user {request.Email} was received.");
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

        logger.LogInformation($"Login response was successfully sent for user {request.Email}");
        return new LoginResponse
        {
            AccessToken = tokensModel.AccessToken,
            RefreshToken = tokensModel.RefreshToken
        };
    }
}