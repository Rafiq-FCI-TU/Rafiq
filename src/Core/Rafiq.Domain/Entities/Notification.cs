using Rafiq.Domain.Common;
namespace Rafiq.Domain.Entities;

public class Notification : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string TypeofNotification  { get; set; }
    public string? Payload { get; set; }  // Extra notification data as JSON (e.g. sessionId, attemptId, score, chatId) for client-side navigation/details

    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}