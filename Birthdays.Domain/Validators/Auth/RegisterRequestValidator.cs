using Domain.DTO;
using Domain.DTO.Requests.Auth;
using FluentValidation;

namespace Domain.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(req => req.Password)
            .NotNull()
            .NotEmpty()
            .Length(8, 125);
        RuleFor(request => request.BirthDate)
            .NotNull();
        RuleFor(request => request.BirthDate)
            .Must(BeValidDate);
        RuleFor(req => req.Password)
            .Must(p => p.Any(char.IsDigit))
            .WithErrorCode("NoDigitsValidator")
            .WithMessage("Password must include at least 1 digit");
    }

    private static bool BeValidDate(DateDto date)
        => date.Year is >= 1 and <= 9999
           && date.Month is >= 1 and <= 12
           && date.Day >= 1
           && date.Day <= DateTime.DaysInMonth(date.Year, date.Month);
}