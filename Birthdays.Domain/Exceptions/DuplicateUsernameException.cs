using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class DuplicateUsernameException : IdentityException
{
    public override int StatusCode => StatusCodes.Status409Conflict;
}