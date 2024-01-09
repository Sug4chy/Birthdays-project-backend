using AutoMapper;
using Data.Context;
using Domain.DTO;
using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Domain.Validators;
using FluentValidation;
using Serilog;

namespace Domain.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService,
    IUserService userService,
    IProfileService profileService,
    AppDbContext context,
    RegisterRequestValidator requestValidator,
    IMapper mapper
)
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        Log.Information($"Register request from user {request.Email} was received.");
        await requestValidator.ValidateAndThrowAsync(request, ct);

        var profile = await profileService.CreateAsync(ct);
        var user = await userService.CreateUserAsync(request, profile, ct);

        var possibleError = await authService.RegisterUserAsync(new RegisterModel
        {
            User = user,
            Password = request.Password
        }, ct);

        if (possibleError is not null)
        {
            Log.Error($"\"{possibleError.Message}\" error was occurred while registering " +
                      $"user {request.Email}");
            IdentityException.ThrowByError(possibleError);
        }

        await context.SaveChangesAsync(ct);

        Log.Information($"Register response was successfully sent for user {request.Email}");
        return new RegisterResponse
        {
            Token = await authService.GenerateToken(user, ct),
            User = mapper.Map<UserDto>(user)
        };
    }
}