using Domain.Results;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class ForbiddenException : ExceptionBase
{
    public override int StatusCode => StatusCodes.Status403Forbidden;
}