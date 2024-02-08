namespace Web.Extensions;

public static class HttpContextExtensions
{
    public static string? GetJwtToken(this HttpContext context)
        => context.Request.Headers.Authorization;
}