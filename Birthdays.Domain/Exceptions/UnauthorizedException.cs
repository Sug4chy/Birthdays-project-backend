using Domain.Results;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class UnauthorizedException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;

    public static void ThrowByError(Error error)
    {
        throw new UnauthorizedException
        {
            Errors = [error]
        };
    }
}