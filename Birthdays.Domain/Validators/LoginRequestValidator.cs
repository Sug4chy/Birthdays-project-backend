using Domain.DTO.Requests.Auth;
using Domain.Results;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Validators;

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(req => req.Password)
            .NotNull()
            .NotEmpty()
            .Length(8, 125);
        RuleFor(req => req.Password)
            .Must(p => p.Any(char.IsDigit))
            .WithErrorCode("NoDigitsValidator")
            .WithMessage("Password must include at least 1 digit");
    }

    public new async Task<Result> ValidateAsync(LoginRequest request, CancellationToken ct = default)
    {
        var result = await base.ValidateAsync(request, ct);
        return Result.FromValidationResult(result);
    }
}