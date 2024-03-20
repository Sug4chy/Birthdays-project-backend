using Domain.DTO.Requests.Auth;
using Domain.Validators.Dto;
using FluentValidation;

namespace Domain.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(DateDtoValidator dateDtoValidator)
    {
        RuleFor(request => request.Name)
            .NotEmpty();
        RuleFor(request => request.Surname)
            .NotEmpty();
        RuleFor(request => request.Patronymic)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrEmptyValidator")
            .WithMessage("Patronymic must be null or non-empty");
        RuleFor(request => request.BirthDate)
            .SetValidator(dateDtoValidator);
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
}