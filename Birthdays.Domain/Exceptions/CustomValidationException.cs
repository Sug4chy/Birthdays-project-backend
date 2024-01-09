using Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class CustomValidationException(Error error) : ValidationException(error.Message,
    new[] { new ValidationFailure { ErrorCode = error.Code, ErrorMessage = error.Message } });