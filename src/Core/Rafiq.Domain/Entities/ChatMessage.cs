using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;

public class ChatMessage : BaseEntity
{
    [Required]
    public string SenderId { get; set; } = string.Empty;

    [ForeignKey(nameof(SenderId))]
    public ApplicationUser Sender { get; set; } = null!;

    [Required]
    public string ReceiverId { get; set; } = string.Empty;

    [ForeignKey(nameof(ReceiverId))]
    public ApplicationUser Receiver { get; set; } = null!;

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
