using FluentValidation;

namespace Domain.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .NotNull()
            .Length(8, int.MaxValue);
        RuleFor(password => password)
            .Must(HasAtListOneDigit)
            .WithErrorCode("NoDigitsValidator");
    }

    private static bool HasAtListOneDigit(string password)
        => password.Any(char.IsDigit);
}