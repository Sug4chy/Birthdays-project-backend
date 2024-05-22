using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class DeleteWishRequestValidator : AbstractValidator<DeleteWishRequest>
{
    public DeleteWishRequestValidator()
    {
        RuleFor(request => request.WishListId)
            .NotEqual(Guid.Empty);
        RuleFor(request => request.WishId)
            .NotEqual(Guid.Empty);
    }
}