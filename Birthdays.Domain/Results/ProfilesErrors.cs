namespace Domain.Results;

public class ProfilesErrors
{
    public static Error NoSuchProfileWithId(Guid id)
        => new("NoSuchProfile", $"Profile with id {id} does not exist");
}