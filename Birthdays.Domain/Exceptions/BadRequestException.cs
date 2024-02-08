using Domain.Results;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class BadRequestException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status400BadRequest;

    public static void ThrowByValidationResult(ValidationResult result)
    {
        if (!result.IsValid)
        {
            throw new BadRequestException
            {
                Errors = [new Error(result.Errors[0].ErrorCode, result.Errors[0].ErrorMessage)]
            };
        }
    }
}