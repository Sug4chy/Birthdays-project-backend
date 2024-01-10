using Domain.Models;

namespace Domain.Exceptions;

public class NotFoundException : CustomExceptionBase
{
    public override int StatusCode => 404;

    public static void ThrowIfNull(object? o, string message)
    {
        if (o is null)
        {
            throw new NotFoundException
            {
                Errors = new[] { new Error { Code = "DataWasNotFound", Message = message } }
            };
        }
    }
}