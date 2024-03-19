using Domain.DTO.Requests.WishLists;
using Domain.Validators.Dto;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishRequestValidator : AbstractValidator<CreateWishRequest>
{
    public CreateWishRequestValidator(WishDtoValidator wishDtoValidator)
    {
        RuleFor(request => request.WishListId)
            .NotNull()
            .NotEqual(Guid.Empty);
        RuleFor(request => request.Wish)
            .SetValidator(wishDtoValidator);
    }
}