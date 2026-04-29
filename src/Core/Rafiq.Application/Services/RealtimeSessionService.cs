using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Hubs;
using Rafiq.Application.Interfaces;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using System.Text.Json;

namespace Rafiq.Application.Services;

public class RealtimeSessionService : IRealtimeSessionService
{
    private readonly IApplicationDbContext _db;
    private readonly IHubContext<AppHub> _hub;
    private readonly INotificationService _notificationService;

    public RealtimeSessionService(
        IApplicationDbContext db,
        IHubContext<AppHub> hub,
        INotificationService notificationService)
    {
        _db = db;
        _hub = hub;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Publishes a new session and notifies all family accounts linked to the specialist via SignalR and DB.
    /// </summary>
    /// <param name="session">The session entity that was just created.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task PublishSessionAsync(Session session, CancellationToken cancellationToken = default)
    {
        var familyIds = await _db.SpecialistPatients
            .Include(sp => sp.Patient)
            .Where(sp => sp.SpecialistId == session.SpecialistProfileId)
            .Select(sp => sp.Patient.FamilyProfileId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var realtimeDto = new NewSessionNotificationDto(
            session.Id,
            session.Title,
            session.ThumbnailUrl,
            "A specialist has published a new training session",
            session.PublishedAt);

        var payload = JsonSerializer.Serialize(new { sessionId = session.Id });

        foreach (var familyId in familyIds)
        {
            // SignalR → فوري
            await _hub.Clients.User(familyId)
                .SendAsync(HubEvents.NewSession, realtimeDto, cancellationToken);

            // DB → persistent
            await _notificationService.SaveAsync(
                userId: familyId,
                title: session.Title,
                message: "A specialist has published a new training session",
                type: "NewSession",
                payload: payload,
                cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    /// Notifies the specialist in real-time and via DB when a patient submits a new attempt.
    /// </summary>
    /// <param name="attempt">The attempt entity including Patient and Session navigation properties.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task NotifyAttemptSubmittedAsync(
        PatientSessionAttempt attempt,
        CancellationToken cancellationToken = default)
    {
        var specialistId = attempt.Session.SpecialistProfileId;

        var realtimeDto = new AttemptSubmittedDto(
            attempt.Id,
            attempt.PatientId,
            $"{attempt.Patient.FirstName} {attempt.Patient.LastName}".Trim(),
            attempt.SessionId,
            attempt.Session.Title,
            "A patient submitted a new attempt for review");

        var payload = JsonSerializer.Serialize(new
        {
            attemptId = attempt.Id,
            patientId = attempt.PatientId,
            sessionId = attempt.SessionId
        });

        await _hub.Clients.User(specialistId)
            .SendAsync(HubEvents.AttemptUploaded, realtimeDto, cancellationToken);

        await _notificationService.SaveAsync(
            userId: specialistId,
            title: $"New attempt by {attempt.Patient.FirstName}",
            message: "A patient submitted a new attempt for review",
            type: "AttemptSubmitted",
            payload: payload,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Notifies the family account when a specialist reviews a patient attempt (Approved / Rejected).
    /// </summary>
    /// <param name="attemptId">The ID of the reviewed attempt.</param>
    /// <param name="feedback">Doctor's feedback message.</param>
    /// <param name="score">Score awarded to the patient (0 if rejected).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task ReviewAttemptAsync(
        int attemptId,
        string feedback,
        int score,
        CancellationToken cancellationToken = default)
    {
        var attempt = await _db.PatientSessionAttempts
            .Include(a => a.Patient)
            .Include(a => a.Session)
            .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);

        if (attempt is null) return;

        var statusMessage = attempt.Status.ToString() switch
        {
            "Approved" => $"Your attempt was approved! You earned {score} points",
            "Rejected" => "Your attempt was rejected. Please check the feedback",
            _ => "Your attempt has been reviewed"
        };

        var realtimeDto = new AttemptReviewedDto(
            attempt.Id,
            attempt.Patient.Id,
            $"{attempt.Patient.FirstName} {attempt.Patient.LastName}".Trim(),
            attempt.Session.Id,
            attempt.Session.Title,
            score,
            feedback,
            status: attempt.Status.ToString(),
            Message: "A specialist reviewed your training video");

        var payload = JsonSerializer.Serialize(new
        {
            attemptId = attempt.Id,
            sessionId = attempt.Session.Id,
            status = attempt.Status.ToString(),
            score,
            feedback
        });

        await _hub.Clients.User(attempt.Patient.FamilyProfileId)
            .SendAsync(HubEvents.AttemptReviewed, realtimeDto, cancellationToken);

        await _notificationService.SaveAsync(
            userId: attempt.Patient.FamilyProfileId,
            title: $"Attempt {attempt.Status}",
            message: statusMessage,
            type: $"Attempt{attempt.Status}",
            payload: payload,
            cancellationToken: cancellationToken);
    }
}