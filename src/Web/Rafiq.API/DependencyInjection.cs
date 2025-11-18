using Rafiq.API.ServicesExtension;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Infrastructure.Services;
using Rafiq.Persistence;

namespace Rafiq.API;
public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

       
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IIQService, IQService>();

        services.AddControllers();

        services.AddSwaggerServices();
    }
}