using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishRequestValidator : AbstractValidator<CreateWishRequest>
{
    public CreateWishRequestValidator()
    {
        RuleFor(request => request.WishListId)
            .NotNull();
        RuleFor(request => request.Wish)
            .NotNull();
        RuleFor(request => request.Wish.Name)
            .NotNull()
            .NotEmpty();
    }
}