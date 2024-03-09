using Domain.DTO.Requests.WishLists;
using Domain.Validators.Dto;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishListRequestValidator : AbstractValidator<CreateWishListRequest>
{
    public CreateWishListRequestValidator()
    {
        RuleFor(request => request.WishList.Name)
            .NotEmpty();
        RuleFor(request => request.WishList.Id)
            .Null();
        RuleFor(request => request.WishList.Description)
            .Must(s => s is null || s.Length != 0);
        RuleForEach(request => request.WishList.Wishes)
            .SetValidator(new WishDtoValidator());
    }
}