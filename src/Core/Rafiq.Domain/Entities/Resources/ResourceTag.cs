using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rafiq.Domain.Entities.Resources;

public class ResourceTag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;

    public int ResourceId { get; set; }

    [ForeignKey(nameof(ResourceId))]
    public Resource Resource { get; set; } = null!;
}
