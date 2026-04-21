using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rafiq.Domain.Entities.Community;

public class CommentReaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public ReactionType Type { get; set; }

    public int CommentId { get; set; }

    [ForeignKey(nameof(CommentId))]
    public Comment Comment { get; set; } = null!;

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser User { get; set; } = null!;
}