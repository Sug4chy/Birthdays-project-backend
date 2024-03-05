using Domain.DTO;
using FluentValidation;

namespace Domain.Validators.WishLists;

public class WishDtoValidator : AbstractValidator<WishDto>
{
    public WishDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty();
        RuleFor(dto => dto.Description)
            .Must(s => s is null || s.Length != 0);
        RuleFor(dto => dto.GiftRef)
            .Must(s => s is null || s.Length != 0);
    }
}