using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Persistence.Data.Configurations;

public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Value)
            .IsRequired()
            .HasMaxLength(100);

        // One Post → Many Tags
        builder.HasOne(t => t.Post)
            .WithMany(p => p.Tags)
            .HasForeignKey(t => t.CommunityPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate tags per post
        builder.HasIndex(t => new { t.CommunityPostId, t.Value })
            .IsUnique()
            .HasDatabaseName("IX_PostTag_Post_Value");
    }
}
