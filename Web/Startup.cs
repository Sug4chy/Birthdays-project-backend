using System.Text;
using Data.Extensions;
using Domain.Configs;
using Domain.Extensions;
using Domain.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BirthdaysApp", Version = "v1" });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please, insert your access token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    }, []
                }
            });
        });

        services.AddDataLayerServices(configuration)
            .WithIdentity();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtConfiguration = configuration.GetSection(JwtConfigurationOptions.Position);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfiguration.GetValue<string>("Issuer"),
                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.GetValue<string>("Audience"),
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            jwtConfiguration.GetValue<string>("SymmetricSecurityKey")!)),
                    ValidateIssuerSigningKey = true
                };
            });
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
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseCors(builder => 
            builder 
                //.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
                .AllowAnyOrigin()
                .WithMethods("GET", "POST", "PUT", "DELETE"));
        
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(e => e.MapControllers());
    }
}