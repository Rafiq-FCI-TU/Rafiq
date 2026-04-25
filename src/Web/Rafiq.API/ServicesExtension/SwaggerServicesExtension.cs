using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using System.Reflection;


namespace Rafiq.API.ServicesExtension;

public static class SwaggerServicesExtension
{
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Rafiq API",
                Description = "API for managing care services for children with Down syndrome",
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = "abdulrahmanfikry1@gmail.com",
                    Url = new Uri("https://yourwebsite.com")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // 🔹  summaries و <example> في Swagger / Redoc
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // 🔹[SwaggerSchema], [SwaggerOperation], [SwaggerResponse] annotations
            options.EnableAnnotations();



            //var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer token\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

        });


    }

    public static void UseSwaggerAndOpenApiMiddleware(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rafiq API");
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            options.RoutePrefix = string.Empty;
            options.ConfigObject.AdditionalItems["persistAuthorization"] = true;
        });
    }
}
