namespace Domain.DTO.Requests.Profiles;

public record SubscribeToRequest
{
    public required Guid BirthdayManId { get; init; }
}