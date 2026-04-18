using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Domain.Entities.Community;
public class PostReaction
{
    [Key]
    public int Id { get; set; }

    public ReactionType Type { get; set; }

    public int CommunityPostId { get; set; }

    [ForeignKey(nameof(CommunityPostId))]
    public CommunityPost Post { get; set; } = null!;

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser User { get; set; } = null!;
}