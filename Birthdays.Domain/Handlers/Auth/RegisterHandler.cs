using Data.Context;
using Domain.Exceptions;
using Domain.Models;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Domain.Validators;
using Serilog;

namespace Domain.Handlers.Auth;

public class RegisterHandler(
    IAuthService authService,
    IUserService userService,
    IProfileService profileService,
    AppDbContext context,
    PasswordValidator passwordValidator
)
{
    public async Task<RegisterResponse> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        Log.Information($"Register request from user {request.Email} was received.");
        var validationResult = await passwordValidator.ValidateAsync(request.Password, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new Error { Code = e.ErrorCode, Message = e.ErrorMessage })
                .ToList();
            Log.Error($"\"{string.Join(", ", errors.Select(e => e.Message))}\" " +
                      $"errors were occurred while validating password for user {request.Email}");
            throw new PasswordValidationException
            {
                Response = new RegisterResponse
                {
                    ProfileId = Guid.Empty,
                    Token = null!
                },
                Errors = errors
            };
        }

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
            throw new DuplicateUsernameException
            {
                Response = new RegisterResponse
                {
                    ProfileId = Guid.Empty,
                    Token = null!
                },
                Error = possibleError
            };
        }

        await context.SaveChangesAsync(ct);

        Log.Information("Register response was successfully sent");
        return new RegisterResponse
        {
            ProfileId = profile.Id,
            Token = await authService.GenerateToken(user, ct)
        };
    }
}