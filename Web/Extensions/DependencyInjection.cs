using Web.Handlers.Auth;
using Web.Services.Auth;
using Web.Services.Profiles;
using Web.Services.Users;

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
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IProfileService, ProfileService>();
        return services;
    }
}