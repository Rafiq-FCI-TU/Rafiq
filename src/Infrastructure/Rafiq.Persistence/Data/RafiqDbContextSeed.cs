using Microsoft.AspNetCore.Identity;
using Rafiq.Domain.Enums;
using System.Text.Json;

namespace Rafiq.Persistence.Data;

// Seed models for JSON deserialization
public class GameLevelSeed
{
    public int DifficultyLevel { get; set; }
    public int RequiredScore { get; set; }
    public int MaxScore { get; set; }
}

public class GameSeed
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<GameLevelSeed> Levels { get; set; } = new();
}

public class GameCategorySeed
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<GameSeed> Games { get; set; } = new();
}

public static class RafiqDbContextSeed
{
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
            var questions = ReadJson<Question>("questions.json");
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

        // Seed Gaming data (Categories → Games → Levels)
        if (!context.GameCategories.Any())
        {
            var seedCategories = ReadJson<GameCategorySeed>("gaming-seed.json");
            foreach (var cat in seedCategories)
            {
                var category = new GameCategory
                {
                    Name = cat.Name,
                    Description = cat.Description
                };

                foreach (var gameSeed in cat.Games)
                {
                    var game = new Game
                    {
                        Title = gameSeed.Title,
                        Description = gameSeed.Description
                    };

                    foreach (var levelSeed in gameSeed.Levels)
                    {
                        game.Levels.Add(new GameLevel
                        {
                            DifficultyLevel = (DifficultyLevel)levelSeed.DifficultyLevel,
                            RequiredScore = levelSeed.RequiredScore,
                            MaxScore = levelSeed.MaxScore
                        });
                    }

                    category.Games.Add(game);
                }

                context.GameCategories.Add(category);
            }
        }

        await context.SaveChangesAsync();
    }

    private static List<T> ReadJson<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName);

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json)!;
    }
}
