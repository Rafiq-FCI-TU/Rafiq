using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Entities.Community;
public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int LikesCount { get; set; }

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