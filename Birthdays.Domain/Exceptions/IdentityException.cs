using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class IdentityException : CustomExceptionBase
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;
}