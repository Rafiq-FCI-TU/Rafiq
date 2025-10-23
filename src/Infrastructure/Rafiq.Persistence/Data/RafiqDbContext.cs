using Microsoft.EntityFrameworkCore;

namespace Rafiq.Persistence.Data;
public class RafiqDbContext : DbContext
{
    public RafiqDbContext(DbContextOptions<RafiqDbContext> options)
            : base(options)
    {
    }
}