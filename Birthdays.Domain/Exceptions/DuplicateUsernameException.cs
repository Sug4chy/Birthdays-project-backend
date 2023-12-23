using Domain.Models;
using Domain.Responses;

namespace Domain.Exceptions;

public class DuplicateUsernameException : Exception
{
    public required IResponse Response { get; init; }
    public required Error Error { get; init; } 
}