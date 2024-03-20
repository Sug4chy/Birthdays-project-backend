﻿using Domain.DTO;
using FluentValidation;

namespace Domain.Validators.Dto;

public class WishDtoValidator : AbstractValidator<WishDto>
{
    public WishDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .Null();
        RuleFor(dto => dto.Name)
            .NotEmpty();
        RuleFor(dto => dto.Description)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrEmptyValidator")
            .WithMessage("Description must be null or non-empty");
        RuleFor(dto => dto.GiftRef)
            .Must(s => s is null || s.Length != 0)
            .WithErrorCode("NullOrEmptyValidator")
            .WithMessage("GiftRef must be null or non-empty");
    }
}