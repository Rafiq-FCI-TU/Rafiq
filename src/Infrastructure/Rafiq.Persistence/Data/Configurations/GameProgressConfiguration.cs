namespace Rafiq.Persistence.Data.Configurations;

public class GameProgressConfiguration : IEntityTypeConfiguration<GameProgress>
{
    public void Configure(EntityTypeBuilder<GameProgress> builder)
    {
        builder.HasKey(gp => gp.Id);

        builder.Property(gp => gp.EarnedPoints)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(gp => gp.PlayedAt)
            .HasDefaultValueSql("GETDATE()");

        // Relation with GameLevel
        builder.HasOne(gp => gp.GameLevel)
            .WithMany(gl => gl.ProgressRecords)
            .HasForeignKey(gp => gp.GameLevelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relation with Patient
        builder.HasOne(gp => gp.Patient)
            .WithMany()
            .HasForeignKey(gp => gp.PatientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
