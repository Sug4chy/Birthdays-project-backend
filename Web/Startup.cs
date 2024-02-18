using Data.Context;
using Data.Extensions;
using Domain.Configs;
using Domain.Extensions;
using Domain.Mapping;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Web.Extensions;
using Web.Middlewares;

namespace Web;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JwtConfigurationOptions>(
            configuration.GetSection(JwtConfigurationOptions.Position));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerWithJwtAuthentication();

        services.AddDataLayerServices(configuration)
            .WithIdentity();

        services.AddConfiguredJwtAuthentication(configuration);
        services.AddAuthorization();

        services.AddCors();

        services.AddControllers();

        services.AddValidators();
        services.AddApplicationServices();
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddHandlers();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        services.AddSingleton<ErrorHandlingMiddleware>();
        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(builder =>
            builder
                //.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
                .AllowAnyOrigin()
                .WithMethods(HttpMethods.Get,
                    HttpMethods.Post,
                    HttpMethods.Put,
                    HttpMethods.Delete));

        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(e => e.MapControllers());
    }
}