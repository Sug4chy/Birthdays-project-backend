using Domain.Results;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class CustomValidationException(Error error) : ValidationException(error.Description,
    new[] { new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Description } })
{
    
}