namespace Domain.Responses;

public record WrapperResponseDto<TResponse> where TResponse : IResponse
{
    public required TResponse Response { get; init; }
    public List<string>? Errors { get; init; }
    public List<string> Links { get; init; } = new();
}