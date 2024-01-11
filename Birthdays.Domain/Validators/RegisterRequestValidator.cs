using Domain.DTO.Requests.Auth;
using Domain.Results;
using FluentValidation;

namespace Domain.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
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
    
    public new async Task<Result> ValidateAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var result = await base.ValidateAsync(request, ct);
        return Result.FromValidationResult(result);
    }
}