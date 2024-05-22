using Domain.DTO;
using FluentValidation;

namespace Domain.Validators.Dto;

public class WishListDtoValidator : AbstractValidator<WishListDto>
{
    public WishListDtoValidator(WishDtoValidator wishDtoValidator)
    {
        RuleFor(dto => dto.Id)
            .Null();
        RuleFor(dto => dto.Name)
            .NotEmpty();
        RuleFor(dto => dto.Description)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrNonEmptyValidator")
            .WithMessage("Description must be null or non-empty");
        RuleForEach(dto => dto.Wishes)
            .SetValidator(wishDtoValidator);
    }
}