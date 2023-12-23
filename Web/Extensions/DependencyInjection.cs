using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Web.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection WithIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();
        return services;
    }
}