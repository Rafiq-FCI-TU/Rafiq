using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Rafiq.Domain.Entities.Community;


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
    public DbSet<Category> Categories { get; set; }
    public DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<GameLevel> GameLevels { get; set; }
    public DbSet<PatientLevel> PatientLevels { get; set; }

    public DbSet<PatientSessionAttempt> PatientSessionAttempts{ get; set; }

    // Community
    public DbSet<CommunityPost> CommunityPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PostReaction> PostReactions { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(RafiqDbContext).Assembly);

        // TPH configuration
        builder.Entity<ApplicationUser>()
            .HasDiscriminator<string>("UserType")
            .HasValue<ApplicationUser>("ApplicationUser")
            .HasValue<FamilyProfile>("FamilyProfile")
            .HasValue<SpecialistProfile>("SpecialistProfile");
    }

}
