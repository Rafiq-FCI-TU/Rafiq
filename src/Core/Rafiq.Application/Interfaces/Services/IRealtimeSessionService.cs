using Rafiq.Domain.Entities;

namespace Rafiq.Application.Interfaces.Services;

public interface IRealtimeSessionService
{
    Task PublishSessionAsync(Session session, CancellationToken cancellationToken = default);
    Task NotifyAttemptSubmittedAsync(PatientSessionAttempt attempt, CancellationToken cancellationToken = default);

    Task ReviewAttemptAsync(int attemptId, string feedback, int score, CancellationToken cancellationToken = default);
}
