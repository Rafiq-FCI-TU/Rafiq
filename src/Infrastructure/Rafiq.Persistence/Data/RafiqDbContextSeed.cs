using Microsoft.AspNetCore.Identity;

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
}