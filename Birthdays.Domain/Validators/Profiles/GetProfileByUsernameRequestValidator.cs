using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class GetProfileByUsernameRequestValidator : AbstractValidator<GetProfileByUsernameRequest>
{
    public GetProfileByUsernameRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotNull()
            .NotEmpty();
        RuleFor(request => request.Jwt)
            .NotNull()
            .NotEmpty();
    }
}