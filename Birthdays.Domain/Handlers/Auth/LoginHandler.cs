using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Users;
using Domain.Validators;
using FluentValidation;
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
        await validator.ValidateAndThrowAsync(request, ct);

        var loginModel = mapper.Map<LoginModel>(request);
        var possibleError = await authService.LoginUserAsync(loginModel, ct);
        if (possibleError is not null)
        {
            Log.Error($"\"{possibleError.Message}\" error was occurred while login " +
                      $"user with email {request.Email}");
            IdentityException.ThrowByError(possibleError);
        }

        var user = await userService.GetUserByEmailAsync(request.Email, ct);
        
        Log.Information($"Login response was successfully sent for user {request.Email}");
        return new LoginResponse
        {
            Token = await authService.GenerateToken(user!, ct),
            User = mapper.Map<UserDto>(user)
        };
    }
}