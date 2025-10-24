using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired();

            // FK and relationship to FamilyProfile (one-to-many)
            builder.HasOne(p => p.FamilyProfile)
                .WithMany(fp => fp.Patients)
                .HasForeignKey(p => p.FamilyProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Assessment)  // Patient has one Assessment
            .WithOne(a => a.Patient)  // Assessment belongs to one Patient
            .HasForeignKey<Assessment>(a => a.PatientId)  // FK on Assessment side
            .OnDelete(DeleteBehavior.Cascade);  // Delete Assessment if Patient deleted


            // Indexes
            builder.HasIndex(p => p.FamilyProfileId);// For family lookups
            builder.HasIndex(p => p.DateOfBirth);// For age queries
        }
    }
}
