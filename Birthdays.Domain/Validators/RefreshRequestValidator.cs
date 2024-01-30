using Domain.DTO.Requests.Auth;
using Domain.Results;
using FluentValidation;

namespace Domain.Validators;

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
    
    public new async Task<Result> ValidateAsync(RefreshRequest request, CancellationToken ct = default)
    {
        var result = await base.ValidateAsync(request, ct);
        return Result.FromValidationResult(result);
    }
}