namespace Rafiq.Persistence.Data.Configurations;

public class GamingConfiguration : IEntityTypeConfiguration<Gaming>
{
    public void Configure(EntityTypeBuilder<Gaming> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // One Gaming → One Patient
        builder.HasOne(g => g.Patient)
            .WithMany()
            .HasForeignKey(g => g.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
