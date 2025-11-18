using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations;
public class SpecialistPatientConfiguration : IEntityTypeConfiguration<SpecialistPatient>
{
    public void Configure(EntityTypeBuilder<SpecialistPatient> builder)
    {

        // Composite primary key
        builder.HasKey(sp => new { sp.SpecialistId, sp.PatientId });

        builder.Property(sp => sp.AssignedAt)
            .IsRequired().HasDefaultValueSql("GETUTCDATE()");

        // Relationships
        // To SpecialistProfile
        builder.HasOne(sp => sp.Specialist)
            .WithMany(s => s.SpecialistPatients)
            .HasForeignKey(sp => sp.SpecialistId)
            .OnDelete(DeleteBehavior.Cascade);
        // To Patient
        builder.HasOne(sp => sp.Patient)
            .WithMany(p => p.SpecialistPatients)
            .HasForeignKey(sp => sp.PatientId)
            .OnDelete(DeleteBehavior.NoAction);

        // Indexes
        builder.HasIndex(sp => sp.SpecialistId);// For specialist lookups
        builder.HasIndex(sp => sp.PatientId);// For patient lookups
    }
}

