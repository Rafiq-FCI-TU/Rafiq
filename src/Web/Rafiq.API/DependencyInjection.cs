using Rafiq.API.ServicesExtension;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Infrastructure.Services;
using Rafiq.Infrastructure.Services.Auth;
using Rafiq.Persistence;

namespace Rafiq.API;
public static class DependencyInjection
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFamilyRegistrationService, FamilyRegistrationService>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();

        services.AddScoped<IIqService, IqService>();

        services.AddControllers();

        services.AddSwaggerServices();
    }
}