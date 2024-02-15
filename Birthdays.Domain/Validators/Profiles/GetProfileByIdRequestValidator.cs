using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class GetProfileByIdRequestValidator : AbstractValidator<GetProfileByIdRequest>
{
    public GetProfileByIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotNull()
            .NotEmpty();
    }
}