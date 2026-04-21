using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Persistence.Data.Configurations;

public class CommunityPostConfiguration : IEntityTypeConfiguration<CommunityPost>
{
    public void Configure(EntityTypeBuilder<CommunityPost> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.AppUserId)
            .IsRequired();

        // One User → Many Posts
        builder.HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // One Post → Many Comments
        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.CommunityPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // One Post → Many Reactions
        builder.HasMany(p => p.Reactions)
            .WithOne(r => r.Post)
            .HasForeignKey(r => r.CommunityPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // One Post → Many Tags
        builder.HasMany(p => p.Tags)
            .WithOne(t => t.Post)
            .HasForeignKey(t => t.CommunityPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
