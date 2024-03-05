using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class UnsubscribeFromRequestValidator : AbstractValidator<UnsubscribeFromRequest>
{
    public UnsubscribeFromRequestValidator()
    {
        RuleFor(request => request.BirthdayManId)
            .NotEqual(Guid.Empty);
    }
}