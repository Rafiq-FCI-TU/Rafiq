namespace Rafiq.Persistence.Data.Configurations;

public class GameLevelConfiguration : IEntityTypeConfiguration<GameLevel>
{
    public void Configure(EntityTypeBuilder<GameLevel> builder)
    {
        builder.HasKey(gl => gl.Id);

        builder.Property(gl => gl.RequiredScore)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(gl => gl.RewardPoints)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(gl => gl.DifficultyLevel)
            .IsRequired();

        // One GameLevel → Many ProgressRecords
        builder.HasMany(gl => gl.ProgressRecords)
            .WithOne(gp => gp.GameLevel)
            .HasForeignKey(gp => gp.GameLevelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
