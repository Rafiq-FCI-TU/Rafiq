using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities.Community;

public class CommunityPost : BaseEntity
{
    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TotalReactionsCount { get; set; }
    public int CommentsCount { get; set; }

    [NotMapped]
    public string PostedAgo => CommunityShared.FormatTimeAgo(CreatedAt);

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser Author { get; set; } = null!;

    // Navigation
    public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();
}