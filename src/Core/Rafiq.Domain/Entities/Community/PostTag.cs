using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rafiq.Domain.Entities.Community;

public class PostTag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;

    public int CommunityPostId { get; set; }

    [ForeignKey(nameof(CommunityPostId))]
    public CommunityPost Post { get; set; } = null!;
}
