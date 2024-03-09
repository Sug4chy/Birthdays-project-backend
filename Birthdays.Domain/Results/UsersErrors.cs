namespace Domain.Results;

public static class UsersErrors
{
    public static Error NoSuchUserWithEmail(string email)
        => new("NoSuchUser", $"User with email {email} does not exist");

    public static Error NoSuchUserWithId(string id)
        => new("NoSuchUser", $"User with id {id} does not exist");
}