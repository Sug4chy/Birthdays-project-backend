namespace Domain.DTO.Requests.Profiles;

public record UnsubscribeFromRequest
{
    public required Guid BirthdayManId { get; init; }
}