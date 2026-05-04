namespace Rafiq.Application.Hubs;

public sealed record ChatMessageDto(
    int Id,
    string SenderId,
    string SenderName,
    string ReceiverId,
    string ReceiverName,
    string Content,
    DateTimeOffset SentAt,
    bool IsRead,
    DateTimeOffset? ReadAt);

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
    string status,
    string Message
    );

public sealed record NewSessionNotificationDto(
    int SessionId,
    string Title,
    string ThumbnailUrl,
    string Message,
    DateTime PublishedAt);


public sealed record AttemptSubmittedDto(
    int AttemptId,
    int PatientId,
    string PatientName,
    int SessionId,
    string SessionTitle,
    string Message
);