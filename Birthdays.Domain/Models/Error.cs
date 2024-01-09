namespace Domain.Models;

public class Error
{
    public required string Code { get; init; }
    public required string Message { get; init; }
}