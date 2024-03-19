using Domain.DTO;
using FluentValidation;

namespace Domain.Validators.Dto;

public class DateDtoValidator : AbstractValidator<DateDto>
{
    public DateDtoValidator()
    {
        RuleFor(dto => dto.Year)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(9999);
        RuleFor(dto => dto.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12);
        RuleFor(dto => dto.Day)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(dto => DateTime.DaysInMonth(dto.Year, dto.Month));
    }
}