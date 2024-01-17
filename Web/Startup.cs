using System.Text;
using Data.Extensions;
using Domain.Configs;
using Domain.Extensions;
using Domain.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        services.AddSwaggerGen();

        services.AddDataLayerServices(configuration)
            .WithIdentity();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Issuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Audience"),
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration.GetValue<string>("SymmetricSecurityKey")!)),
                    ValidateIssuerSigningKey = true
                };
            });

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
    }

    public void Configure(IApplicationBuilder app)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(e => e.MapControllers());
    }
}