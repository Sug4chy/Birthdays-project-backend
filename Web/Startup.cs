using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var updateAuditableInterceptor = serviceProvider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(updateAuditableInterceptor);
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
    }
}