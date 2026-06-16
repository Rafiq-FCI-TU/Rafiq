using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafiq.Domain.Common;
using Rafiq.Domain.Entities;

namespace Rafiq.Domain.Entities.Resources;

public class Resource : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2048)]
    public string Link { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string AppUserId { get; set; } = string.Empty;

    [ForeignKey(nameof(AppUserId))]
    public ApplicationUser Author { get; set; } = null!;

    public ICollection<ResourceTag> Tags { get; set; } = new List<ResourceTag>();
}
