namespace Domain.DTO;

public record DateDto
{
    public required int Year { get; init; }
    public required int Month { get; init; }
    public required int Day { get; init; }
}