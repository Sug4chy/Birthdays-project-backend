using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Domain.Results;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    
    private Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    
    public static Result Failure(Error error) => new(false, error);

    public static Result FromValidationResult(ValidationResult validationResult)
        => validationResult.IsValid
            ? Success()
            : Failure(Error.FromValidationFailure(validationResult.Errors[0]));
    
    public static Result FromIdentityError(IdentityError error)
        => new(false, Error.FromIdentityError(error));
}