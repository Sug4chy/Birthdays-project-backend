﻿using Domain.DTO.Requests.WishLists;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class CreateWishRequestValidator : AbstractValidator<CreateWishRequest>
{
    public CreateWishRequestValidator()
    {
        RuleFor(request => request.WishListId)
            .NotNull()
            .NotEqual(Guid.Empty);
        RuleFor(request => request.Wish.Name)
            .NotEmpty();
        RuleFor(request => request.Wish.Description)
            .Must(s => s is null || s.Length != 0);
        RuleFor(request => request.Wish.GiftRef)
            .Must(s => s is null || s.Length != 0);
    }
}