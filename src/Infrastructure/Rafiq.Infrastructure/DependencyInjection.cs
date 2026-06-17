using Microsoft.Extensions.DependencyInjection;
using Rafiq.Application.Games;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Infrastructure.Games;
using Rafiq.Persistence;

namespace Rafiq.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGameLevelService, GameLevelService>();
        services.AddScoped<IPatientLevelService, PatientLevelService>();
        return services;
    }
}
