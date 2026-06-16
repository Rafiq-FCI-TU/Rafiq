namespace Rafiq.Application.Hubs;

public sealed record SendMessageRequest(string ReceiverId, string Content);

public sealed record NotifyAttemptRequest(int AttemptId, int PatientId, string SpecialistId, int SessionId);
