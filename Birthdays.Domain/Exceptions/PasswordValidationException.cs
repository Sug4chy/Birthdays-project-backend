using Domain.Models;
using Domain.Responses;

namespace Domain.Exceptions;

public class PasswordValidationException : Exception
{
    public required IResponse Response { get; init; } = null!;
    public required IReadOnlyList<Error> Errors { get; init; } = Array.Empty<Error>();
}