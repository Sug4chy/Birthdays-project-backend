using Domain.DTO;
using Domain.DTO.Requests.Auth;
using FluentValidation;

namespace Domain.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty();
        RuleFor(request => request.Surname)
            .NotEmpty();
        RuleFor(request => request.Patronymic)
            .Must(BeNullOrNotEmpty);
        RuleFor(request => request.BirthDate)
            .Must(BeValidDate);
        RuleFor(request => request.Email)
            .NotEmpty();
        RuleFor(req => req.Password)
            .NotEmpty()
            .Length(8, 125);
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

    private static bool BeNullOrNotEmpty(string? s)
        => s is null || s.Length != 0;
}