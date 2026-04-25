using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Hubs;
using Rafiq.Application.Interfaces;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Application.Services;

public class RealtimeSessionService : IRealtimeSessionService
{
    private readonly IApplicationDbContext _db;
    private readonly IHubContext<AppHub> _hub;

    public RealtimeSessionService(IApplicationDbContext db, IHubContext<AppHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    /// <summary>
    /// Publishes a session and notifies all linked family accounts for the specialist.
    /// </summary>
    public async Task PublishSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        //_db.Sessions.Add(session);
        await _db.SaveChangesAsync(cancellationToken);

        var familyIds = await _db.SpecialistPatients
            .Include(sp => sp.Patient)
            .Where(sp => sp.SpecialistId == session.SpecialistProfileId)
            .Select(sp => sp.Patient.FamilyProfileId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var notification = new NewSessionNotificationDto(
            session.Id,
            session.Title,
            session.ThumbnailUrl,
            "الدكتور نشر جلسة تدريبية جديدة",
            session.PublishedAt);

        foreach (var familyId in familyIds)
        {
            await _hub.Clients.User(familyId)
                .SendAsync(HubEvents.NewSession, notification, cancellationToken);
        }
    }

    /// <summary>
    /// Marks an attempt as reviewed and notifies the owning family account.
    /// </summary>
    public async Task ReviewAttemptAsync(int attemptId, string feedback, int score, CancellationToken cancellationToken = default)
    {
        var attempt = await _db.PatientSessionAttempts
            .Include(a => a.Patient)
            .Include(a => a.Session)
            .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);

        if (attempt is null)
            return;

        attempt.DoctorFeedback = feedback;
        attempt.ScoreAwarded = score;
        attempt.Status = AttemptStatus.Reviewed;
        attempt.ReviewedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        var notification = new AttemptReviewedDto(
            attempt.Id,
            attempt.Patient.Id,
            $"{attempt.Patient.FirstName} {attempt.Patient.LastName}".Trim(),
            attempt.Session.Id,
            attempt.Session.Title,
            score,
            feedback,
            "الدكتور راجع فيديو التدريب");

        await _hub.Clients.User(attempt.Patient.FamilyProfileId)
            .SendAsync(HubEvents.AttemptReviewed, notification, cancellationToken);
    }
}
