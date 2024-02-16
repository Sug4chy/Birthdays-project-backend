using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishListRequestValidator : AbstractValidator<CreateWishListRequest>
{
    public CreateWishListRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotNull()
            .NotEmpty();
    }
}