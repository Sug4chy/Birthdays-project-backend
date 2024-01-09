using Domain.Models;

namespace Domain.DTO.Responses;

public record WrapperResponseDto<TResponse> where TResponse : IResponse
{
    public required TResponse? Response { get; init; }
    public Error[]? Errors { get; init; }
    public string[]? Links { get; init; }
}