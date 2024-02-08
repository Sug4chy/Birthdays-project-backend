using Domain.DTO.Requests.Auth;
using Domain.Results;
using FluentValidation;

namespace Domain.Validators.Auth;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotNull()
            .NotEmpty();
        RuleFor(request => request.ExpiredAccessToken)
            .NotNull()
            .NotEmpty();
    }
}