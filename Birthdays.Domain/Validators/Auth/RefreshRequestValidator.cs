using Domain.DTO.Requests.Auth;
using FluentValidation;

namespace Domain.Validators.Auth;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty();
        RuleFor(request => request.ExpiredAccessToken)
            .NotEmpty();
    }
}