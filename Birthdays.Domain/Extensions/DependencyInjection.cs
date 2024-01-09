using Domain.Handlers.Auth;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Users;
using Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class DependencyInjection
{
    
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<RegisterRequestValidator>();
        services.AddScoped<LoginRequestValidator>();
        return services;
    }

}