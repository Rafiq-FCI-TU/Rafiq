using Microsoft.AspNetCore.Identity;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Persistence.Data;
public class RafiqDbContextSeed
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

    public static List<GameCategory> GetGameCategories()
    {
        return
        [
            new GameCategory { Id = 1, Name = "Cognitive Skills", InitialScore = 0 },
            new GameCategory { Id = 2, Name = "Memory Training", InitialScore = 0 }
        ];
    }

    public static List<Game> GetGames()
    {
        return
        [
            // Cognitive Skills games
            new Game { Id = 1, CategoryId = 1, Title = "Match Shapes", MaxPoints = 100, Duration = "5 mins", Status = GameStatus.NotStarted },
            new Game { Id = 2, CategoryId = 1, Title = "Logic Puzzle", MaxPoints = 200, Duration = "10 mins", Status = GameStatus.NotStarted },
            // Memory Training games
            new Game { Id = 3, CategoryId = 2, Title = "Card Flip", MaxPoints = 150, Duration = "8 mins", Status = GameStatus.NotStarted }
        ];
    }
}