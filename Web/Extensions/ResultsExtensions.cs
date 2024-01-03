using Domain.Responses;

namespace Web.Extensions;

public static class ResultsExtensions
{
    public static async Task<WrapperResponseDto<TResponse>> WrappedWithLinks<TResponse>(
        this Task<TResponse> response) 
        where TResponse : IResponse
    {
        var taskResult = await response;
        return new WrapperResponseDto<TResponse>
        {
            Response = taskResult,
            Errors = null,
            Links = new List<string>()
        };
    }
}