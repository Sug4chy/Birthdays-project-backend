namespace Domain.Results;

public static class AuthErrors
{
    public static readonly Error SignInError 
        = new("LoginOrPasswordInvalid", "Login or/and password is/are not valid");

    public static readonly Error InvalidRefreshToken
        = new(nameof(InvalidRefreshToken), "Presented refresh token isn't valid");

    public static readonly Error InvalidAccessToken
        = new(nameof(InvalidAccessToken), "Presented access token isn't valid");

    public static Error UnauthenticatedError(string email)
        => new(nameof(UnauthenticatedError), 
            $"User with email {email} is not authenticated");

    public static Error DoesNotIncludeClaim(string claimType)
        => new("NotIncludeClaim", $"Authentication token must include claim with type {claimType}");
}