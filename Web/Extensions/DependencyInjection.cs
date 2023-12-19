using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection WithIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        return services;
    }
}