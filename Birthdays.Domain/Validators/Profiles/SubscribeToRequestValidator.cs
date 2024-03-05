using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class SubscribeToRequestValidator : AbstractValidator<SubscribeToRequest>
{
    public SubscribeToRequestValidator()
    {
        RuleFor(request => request.BirthdayManId)
            .NotEqual(Guid.Empty);
    }
}