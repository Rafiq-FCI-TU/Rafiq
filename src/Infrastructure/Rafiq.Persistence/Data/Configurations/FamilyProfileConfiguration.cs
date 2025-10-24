using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Common;

namespace Rafiq.Persistence.Data.Configurations
{
    public class FamilyProfileConfiguration : IEntityTypeConfiguration<FamilyProfile>
    {
        public void Configure(EntityTypeBuilder<FamilyProfile> builder)
        {

            builder.Property(fp => fp.Relationship)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(fp => fp.EmergencyContactName)
                .HasMaxLength(100);

            builder.Property(fp => fp.EmergencyContactPhone)
                .HasMaxLength(20);
        }
    }

}
