using Domain.Models;

namespace Domain.Responses;

public record WrapperResponseDto<TResponse> where TResponse : IResponse
{
    public required TResponse? Response { get; init; }
    public IEnumerable<Error>? Errors { get; init; }
    public IEnumerable<string>? Links { get; init; }
}