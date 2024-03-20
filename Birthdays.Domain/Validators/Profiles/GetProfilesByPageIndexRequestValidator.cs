using Domain.DTO.Requests.Profiles;
using FluentValidation;

namespace Domain.Validators.Profiles;

public class GetProfilesByPageIndexRequestValidator : AbstractValidator<GetProfilesByPageIndexRequest>
{
    public GetProfilesByPageIndexRequestValidator()
    {
        RuleFor(request => request.PageIndex)
            .GreaterThanOrEqualTo(0);
    }
}