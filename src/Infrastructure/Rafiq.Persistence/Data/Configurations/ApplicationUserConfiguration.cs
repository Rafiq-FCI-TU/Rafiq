using Rafiq.Domain.Enums;

namespace Rafiq.Persistence.Data.Configurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.Address)
            .HasMaxLength(200);


        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");


        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);  // Optional;

        // Auto-generate discriminator values

        builder.HasDiscriminator<UserRole>("Role")  // Enum type, column name "UserType" 
            .HasValue<FamilyProfile>(UserRole.Family)      // Maps to 0
            .HasValue<SpecialistProfile>(UserRole.Specialist); // Maps to 1
                                                               //.HasValue<AdminProfile>(UserRole.Admin); // Maps to 2

    }
}
