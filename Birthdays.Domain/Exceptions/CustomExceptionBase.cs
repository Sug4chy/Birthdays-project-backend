using Domain.Models;
using Domain.Results;

namespace Domain.Exceptions;

public abstract class CustomExceptionBase : Exception
{
    public abstract int StatusCode { get; }
    public IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();
}