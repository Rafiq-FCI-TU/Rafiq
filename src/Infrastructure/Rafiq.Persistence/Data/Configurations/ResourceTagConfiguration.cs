using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Resources;

namespace Rafiq.Persistence.Data.Configurations;

public class ResourceTagConfiguration : IEntityTypeConfiguration<ResourceTag>
{
    public void Configure(EntityTypeBuilder<ResourceTag> builder)
    {
        builder.HasKey(tag => tag.Id);

        builder.Property(tag => tag.Value)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(tag => tag.Resource)
            .WithMany(resource => resource.Tags)
            .HasForeignKey(tag => tag.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tag => new { tag.ResourceId, tag.Value })
            .IsUnique()
            .HasDatabaseName("IX_ResourceTag_Resource_Value");
    }
}
