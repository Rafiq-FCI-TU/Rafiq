using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Rafiq.Persistence.Data;
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

    public static async Task SeedAsync(RafiqDbContext context)
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

        await context.SaveChangesAsync();
    }

    private static List<T> ReadJson<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName);

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json)!;
    }
}