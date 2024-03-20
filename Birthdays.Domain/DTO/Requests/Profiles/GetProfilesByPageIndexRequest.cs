namespace Domain.DTO.Requests.Profiles;

public record GetProfilesByPageIndexRequest
{
    public required int PageIndex { get; init; }
}