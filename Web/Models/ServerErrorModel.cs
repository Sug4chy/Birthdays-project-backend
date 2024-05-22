using Domain.Results;

namespace Web.Models;

public class ServerErrorModel(Error error)
{
    public Error Error { get; init; } = error;
}