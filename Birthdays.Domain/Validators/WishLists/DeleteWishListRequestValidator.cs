using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class DeleteWishListRequestValidator : AbstractValidator<DeleteWishListRequest>
{
    public DeleteWishListRequestValidator()
    {
        RuleFor(request => request.WishListId)
            .NotEqual(Guid.Empty);
    }
}