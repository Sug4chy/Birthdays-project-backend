using Domain.DTO.Requests.WishLists;
using Domain.Validators.Dto;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class UpdateWishRequestValidator : AbstractValidator<UpdateWishRequest>
{
    public UpdateWishRequestValidator(WishDtoValidator wishDtoValidator)
    {
        RuleFor(request => request.WishListId)
            .NotNull()
            .NotEqual(Guid.Empty);
        RuleFor(request => request.WishId)
            .NotNull()
            .NotEqual(Guid.Empty);
        RuleFor(request => request.Wish)
            .SetValidator(wishDtoValidator);
    }
}