using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Persistence.Data.Configurations;

public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
{
    public void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.AppUserId)
            .IsRequired();

        builder.Property(r => r.Type)
            .IsRequired();

        // One User → Many CommentReactions
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index: one reaction per user per comment (enforces at DB level)
        builder.HasIndex(r => new { r.AppUserId, r.CommentId })
            .IsUnique()
            .HasDatabaseName("IX_CommentReaction_User_Comment");
    }
}
