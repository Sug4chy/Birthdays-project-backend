using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class GetCurrentProfileRequestValidator : AbstractValidator<GetCurrentProfileRequest>
{
    public GetCurrentProfileRequestValidator()
    {
        RuleFor(request => request.Jwt)
            .NotNull()
            .NotEmpty();
    }
}