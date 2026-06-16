using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Persistence.Data.Configurations;

public class PostReactionConfiguration : IEntityTypeConfiguration<PostReaction>
{
    public void Configure(EntityTypeBuilder<PostReaction> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.AppUserId)
            .IsRequired();

        builder.Property(r => r.Type)
            .IsRequired();

        // One User → Many PostReactions
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index: one reaction per user per post (enforces at DB level)
        builder.HasIndex(r => new { r.AppUserId, r.CommunityPostId })
            .IsUnique()
            .HasDatabaseName("IX_PostReaction_User_Post");
    }
}
