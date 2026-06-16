using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Resources;

namespace Rafiq.Persistence.Data.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(resource => resource.Id);

        builder.Property(resource => resource.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(resource => resource.Link)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(resource => resource.Description)
            .HasMaxLength(2000);

        builder.Property(resource => resource.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(resource => resource.AppUserId)
            .IsRequired();

        builder.HasOne(resource => resource.Author)
            .WithMany()
            .HasForeignKey(resource => resource.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(resource => resource.Tags)
            .WithOne(tag => tag.Resource)
            .HasForeignKey(tag => tag.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
