using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Rafiq.API.ServicesExtension;
public static class SwaggerServicesExtension
{
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddOpenApi();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer token\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void UseSwaggerAndOpenApiMiddleware(this WebApplication app)
    {
        app.MapOpenApi(); // https://localhost:7025/openapi/v1.json

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rafiq API");
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            options.RoutePrefix = string.Empty;
        });
    }

}