using Domain.Models;

namespace Domain.Exceptions;

public abstract class CustomExceptionBase : Exception
{
    public abstract int StatusCode { get; }
    public IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();
}