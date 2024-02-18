using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishListRequestValidator : AbstractValidator<CreateWishListRequest>
{
    public CreateWishListRequestValidator()
    {
        RuleFor(request => request.WishList.Name)
            .NotNull()
            .NotEmpty();
        RuleFor(request => request.WishList.Wishes)
            .NotNull();
    }
}