using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class GetProfileWishListsByIdRequestValidator : AbstractValidator<GetProfileWishListsByIdRequest>
{
    public GetProfileWishListsByIdRequestValidator()
    {
        RuleFor(request => request.UserId)
            .NotNull();
    }
}