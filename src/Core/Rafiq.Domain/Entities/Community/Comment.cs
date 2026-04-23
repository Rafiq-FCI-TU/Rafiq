using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities.Community;

public class Comment : BaseEntity
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TotalReactionsCount { get; set; }

    [NotMapped]
    public string PostedAgo => CommunityShared.FormatTimeAgo(CreatedAt);

    public int CommunityPostId { get; set; }

    [ForeignKey(nameof(CommunityPostId))]
    public CommunityPost Post { get; set; } = null!;

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser Author { get; set; } = null!;

    // Navigation
    public ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();
}