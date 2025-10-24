using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Persistence.Data.Configurations
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(AssessmentStatus.InProgress);

            builder.Property(a => a.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(a => a.CompletedAt);

            // Relationships
            builder.HasOne(a => a.Patient)
            .WithOne(p => p.Assessment)
            .HasForeignKey<Assessment>(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.PatientId);

        }
    }
}

