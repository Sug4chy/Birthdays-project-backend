using Domain.Handlers.Auth;
using Domain.Handlers.Profiles;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Validators.Auth;
using Domain.Validators.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class DependencyInjection
{
    
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<LogoutHandler>();
        services.AddScoped<RefreshHandler>();

        services.AddScoped<GetProfileByIdHandler>();
        services.AddScoped<GetCurrentProfileHandler>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISubscriptionsService, SubscriptionsService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<RegisterRequestValidator>();
        services.AddScoped<LoginRequestValidator>();
        services.AddScoped<RefreshRequestValidator>();

        services.AddScoped<GetProfileByIdRequestValidator>();
        return services;
    }

}