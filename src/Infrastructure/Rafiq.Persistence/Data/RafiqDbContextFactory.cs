using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Rafiq.Persistence.Data;

public class RafiqDbContextFactory : IDesignTimeDbContextFactory<RafiqDbContext>
{
    public RafiqDbContext CreateDbContext(string[] args)
    {
        var environmentName =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
            "Development";

        var apiProjectPath = ResolveApiProjectPath();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(apiProjectPath, "appsettings.json"), optional: false)
            .AddJsonFile(Path.Combine(apiProjectPath, $"appsettings.{environmentName}.json"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<RafiqDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new RafiqDbContext(optionsBuilder.Options);
    }

    private static string ResolveApiProjectPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directApiPath = Path.Combine(currentDirectory, "src", "Web", "Rafiq.API");
        if (Directory.Exists(directApiPath))
        {
            return directApiPath;
        }

        var siblingApiPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "Web", "Rafiq.API"));
        if (Directory.Exists(siblingApiPath))
        {
            return siblingApiPath;
        }

        throw new DirectoryNotFoundException("Could not locate the Rafiq.API project for design-time configuration.");
    }
}
