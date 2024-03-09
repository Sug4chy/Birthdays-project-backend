using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class UpdateWishListRequestValidator : AbstractValidator<UpdateWishListRequest>
{
    public UpdateWishListRequestValidator()
    {
        RuleFor(request => request.WishListId)
            .NotNull()
            .NotEqual(Guid.Empty);
        RuleFor(request => request.NewDescription)
            .Must(s => s is null || s.Length != 0);
        RuleFor(request => request.NewName)
            .NotEmpty();
    }
}