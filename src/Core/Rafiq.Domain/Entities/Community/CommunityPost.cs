using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Entities.Community;
public class CommunityPost
{
    public int Id { get; set; }

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public List<string>? Tags { get; set; }

    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }

    [NotMapped]
    public string PostedAgo => CommunityShared.FormatTimeAgo(CreatedAt);

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser Author { get; set; } = null!;

    // Navigation
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PostReaction> Reactions { get; set; } = [];
}