using Rafiq.API.ServicesExtension;

namespace Rafiq.API;
public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddControllers();

        services.AddSwaggerServices();
    }
}