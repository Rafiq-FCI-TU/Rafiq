using Microsoft.AspNetCore.Identity;
using Rafiq.Domain.Enums;
using System.Text.Json;

namespace Rafiq.Persistence.Data;
public static class RafiqDbContextSeed
{
    public static async Task SeedAsync(RafiqDbContext context, RoleManager<IdentityRole> roleManager)
    {
        if (!context.Categories.Any())
        {
            var categories = ReadJson<Category>("categories.json");
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        if (!context.Questions.Any())
        {
            var questions = ReadJson<Question>("assessment-questions.json");
            context.Questions.AddRange(questions);
        }

        if (!roleManager.Roles.Any())
        {
            var roles = GetRoles();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        if (!context.Games.Any())
        {
            var games = ReadJson<Game>("games.json");

            context.Games.AddRange(games);

            await context.SaveChangesAsync();
        }

        if (!context.GameLevels.Any() && context.Games.Any())
        {
            var gameLevels = ReadJson<GameLevel>("game-levels.json");

            context.GameLevels.AddRange(gameLevels);

            await context.SaveChangesAsync();
        }

        if (!context.GameQuestions.Any() && context.GameLevels.Any())
        {
            var gameQuestions = ReadJson<GameQuestion>("game-questions.json");
            context.GameQuestions.AddRange(gameQuestions);
            await context.SaveChangesAsync();
        }

        if (!context.GameOptions.Any() && context.GameQuestions.Any())
        {
            var gameOptions = ReadJson<GameOption>("game-options.json");
            context.GameOptions.AddRange(gameOptions);
            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }

    public static List<IdentityRole> GetRoles()
    {
        return
        [
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Family",
                NormalizedName = "FAMILY",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },

            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Specialist",
                NormalizedName = "SPECIALIST",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        ];
    }

    private static List<T> ReadJson<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName);

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json)!;
    }
}
