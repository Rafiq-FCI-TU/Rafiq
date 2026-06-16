namespace Rafiq.Persistence.Data.Configurations;

public class GameProgressConfiguration : IEntityTypeConfiguration<PatientLevel>
{
    public void Configure(EntityTypeBuilder<PatientLevel> builder)
    {
        builder.HasKey(gp => gp.Id);

        builder.Property(gp => gp.EarnedPoints)
            //.IsRequired()
            .HasDefaultValue(0);

        builder.Property(gp => gp.PlayedAt)
            .HasDefaultValueSql("GETDATE()");



        // Relation with Patient
        builder.HasOne(gp => gp.Patient)
            .WithMany()
            .HasForeignKey(gp => gp.PatientId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(gp => gp.GameLevel)
            .WithMany(gl => gl.PatientLevels)
            .HasForeignKey(gp => gp.GameLevelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
