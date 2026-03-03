using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Rafiq.Persistence.Data;
public class RafiqDbContext(DbContextOptions<RafiqDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // DbSets
    public DbSet<FamilyProfile> FamilyProfiles { get; set; }
    public DbSet<SpecialistProfile> SpecialistProfiles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<SpecialistPatient> SpecialistPatients { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Gaming> Gamings { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameProgress> GameProgresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(RafiqDbContext).Assembly);

        builder.Entity<IdentityRole>().HasData(RafiqDbContextSeed.GetRoles());

        // TPH configuration
        builder.Entity<ApplicationUser>()
            .HasDiscriminator<string>("UserType")
            .HasValue<ApplicationUser>("ApplicationUser")
            .HasValue<FamilyProfile>("FamilyProfile")
            .HasValue<SpecialistProfile>("SpecialistProfile");
    }

}
