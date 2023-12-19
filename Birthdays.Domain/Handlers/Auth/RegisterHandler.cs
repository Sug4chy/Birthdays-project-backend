using Data.Context;
using Domain.Models;
using Domain.Requests.Auth;
using Domain.Responses;
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
    public async Task<WrapperResponseDto<RegisterResponse>> Handle(
        RegisterRequest request, CancellationToken ct = default)
    {
        Log.Information("Register request was received.");
        var validationResult = await passwordValidator.ValidateAsync(request.Password, ct);
        if (!validationResult.IsValid)
        {
            var errorsAsStrings = validationResult.Errors
                .Select(e => new Error { Code = e.ErrorCode, Message = e.ErrorMessage })
                .ToList();
            Log.Error($"{string.Join(", ", errorsAsStrings)} errors were occurred");
            return new WrapperResponseDto<RegisterResponse>
            {
                Response = new RegisterResponse
                {
                    ProfileId = Guid.Empty,
                    Token = ""
                },
                Errors = errorsAsStrings
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
            Log.Error($"{possibleError} error was occurred");
            return new WrapperResponseDto<RegisterResponse>
            {
                Response = null!,
                Errors = new List<Error> { possibleError }
            };
        }

        await context.SaveChangesAsync(ct);

        Log.Information("Register response was successfully sent");
        return new WrapperResponseDto<RegisterResponse>
        {
            Response = new RegisterResponse
            {
                ProfileId = profile.Id,
                Token = await authService.GenerateToken(user, ct)
            },
            Errors = null
        };
    }
}