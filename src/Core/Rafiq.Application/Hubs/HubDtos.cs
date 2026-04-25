namespace Rafiq.Application.Hubs;

public sealed record ChatMessageDto(
    int Id,
    string SenderId,
    string SenderName,
    string ReceiverId,
    string Content,
    DateTime SentAt,
    bool IsRead,
    DateTime? ReadAt);

public sealed record AttemptNotificationDto(
    int AttemptId,
    int PatientId,
    string PatientName,
    int SessionId,
    string Message);

public sealed record AttemptReviewedDto(
    int AttemptId,
    int PatientId,
    string PatientName,
    int SessionId,
    string SessionTitle,
    int Score,
    string Feedback,
    string Message);

public sealed record NewSessionNotificationDto(
    int SessionId,
    string Title,
    string ThumbnailUrl,
    string Message,
    DateTime PublishedAt);
