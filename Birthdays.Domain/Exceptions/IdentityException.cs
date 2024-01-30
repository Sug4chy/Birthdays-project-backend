using Domain.Results;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class IdentityException : CustomExceptionBase
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;

    public static void ThrowByError(Error error)
    {
        ThrowIfDuplicate(error);
        ThrowIfInvalid(error);
    }

    private static void ThrowIfDuplicate(Error error)
    {
        if (error.Code.Contains("Duplicate"))
        {
            throw new DuplicateUsernameException
            {
                Errors = new[] { error }
            };
        }
    }

    private static void ThrowIfInvalid(Error error)
    {
        if (error.Code.Contains("Invalid"))
        {
            throw new CustomValidationException(error);
        }
    }
}