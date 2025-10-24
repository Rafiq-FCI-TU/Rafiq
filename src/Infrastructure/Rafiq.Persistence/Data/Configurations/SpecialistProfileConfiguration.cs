using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations
{
    public class SpecialistProfileConfiguration : IEntityTypeConfiguration<SpecialistProfile>
    {
        public void Configure(EntityTypeBuilder<SpecialistProfile> builder)
        {
            builder.Property(sp => sp.Credentials)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(sp => sp.Specialty)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(sp => sp.Organization)
                .HasMaxLength(200);

            builder.Property(sp => sp.ProfessionalBio)
                .HasMaxLength(500);
        }
    }
}
