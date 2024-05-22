using Domain.DTO.Requests.WishLists;
using Domain.Validators.Dto;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishListRequestValidator : AbstractValidator<CreateWishListRequest>
{
    public CreateWishListRequestValidator(WishListDtoValidator wishListDtoValidator)
    {
        RuleFor(request => request.WishList)
            .SetValidator(wishListDtoValidator);
    }
}