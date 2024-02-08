using Domain.Results;

namespace Domain.Exceptions;

public abstract class ExceptionBase : Exception
{
    public abstract int StatusCode { get; }
    public IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();
}