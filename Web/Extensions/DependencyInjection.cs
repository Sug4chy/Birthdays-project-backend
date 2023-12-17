using Web.Handlers.Auth;
using Web.Services;
using Web.Services.Auth;

namespace Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddTransient<RegisterHandler>();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        return services;
    }
}