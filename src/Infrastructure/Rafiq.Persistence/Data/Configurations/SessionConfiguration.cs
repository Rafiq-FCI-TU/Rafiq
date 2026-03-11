namespace Rafiq.Persistence.Data.Configurations;
public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        // Relation: Session → SpecialistProfile (Many-to-One)
        builder
            .HasOne(s => s.Specialist)
            .WithMany(sp => sp.Sessions)
            .HasForeignKey(s => s.SpecialistProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(s => s.Duration)
       .HasConversion<TimeSpan>();

        builder.Property(s => s.VideoUrl)
            .IsRequired();
    }
}
