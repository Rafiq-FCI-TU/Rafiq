using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Interfaces;
using Rafiq.Domain.Entities;

namespace Rafiq.Application.Hubs;

[Authorize]
public class AppHub : Hub
{
    private readonly IApplicationDbContext _db;

    public AppHub(IApplicationDbContext db)
    {
        _db = db;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (string.IsNullOrWhiteSpace(userId))
        {
            await base.OnConnectedAsync();
            return;
        }

        var specialistGroups = await _db.SpecialistPatients
            .AsNoTracking()
            .Where(sp => sp.SpecialistId == userId)
            .Select(sp => GroupName(sp.SpecialistId, sp.PatientId))
            .ToListAsync();

        foreach (var groupName in specialistGroups)
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var familyGroups = await _db.SpecialistPatients
            .AsNoTracking()
            .Include(sp => sp.Patient)
            .Where(sp => sp.Patient.FamilyProfileId == userId)
            .Select(sp => GroupName(sp.SpecialistId, sp.PatientId))
            .ToListAsync();

        foreach (var groupName in familyGroups)
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(SendMessageRequest req)
    {
        var senderId = Context.UserIdentifier;
        if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(req.Content))
            return;

        var sender = await _db.Users.FindAsync(senderId);
        if (sender is null)
            return;

        var message = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = req.ReceiverId,
            Content = req.Content.Trim(),
            SentAt = DateTime.UtcNow
        };
        
        var receiver = await _db.Users.FindAsync(req.ReceiverId);
        if (receiver is null)
            return;

        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync();

        var dto = new ChatMessageDto(
            message.Id,
            senderId,
            $"{sender.FirstName} {sender.LastName}".Trim(),
            req.ReceiverId,
            $"{receiver.FirstName} {receiver.LastName}".Trim(),
            message.Content,
            message.SentAt,
            message.IsRead,
            message.ReadAt);

        await Clients.User(req.ReceiverId).SendAsync(HubEvents.NewMessage, dto);
        await Clients.Caller.SendAsync(HubEvents.NewMessage, dto);
    }

    public async Task NotifyAttemptSubmitted(NotifyAttemptRequest req)
    {
        var familyId = Context.UserIdentifier;
        if (string.IsNullOrWhiteSpace(familyId))
            return;

        var relation = await _db.SpecialistPatients
            .Include(sp => sp.Patient)
            .FirstOrDefaultAsync(sp =>
                sp.PatientId == req.PatientId &&
                sp.SpecialistId == req.SpecialistId &&
                sp.Patient.FamilyProfileId == familyId);

        if (relation is null)
            return;

        var attempt = await _db.PatientSessionAttempts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == req.AttemptId && a.PatientId == req.PatientId && a.SessionId == req.SessionId);

        if (attempt is null)
            return;

           var dto = new AttemptNotificationDto(
       attempt.Id,
       relation.Patient.Id,
       $"{relation.Patient.FirstName} {relation.Patient.LastName}".Trim(),
       req.SessionId,
       "Patient has uploaded a training video — awaiting your review.");
      
           await Clients.User(req.SpecialistId).SendAsync(HubEvents.AttemptUploaded, dto);
       }

    private static string GroupName(string specialistId, int patientId)
        => $"spec_{specialistId}_pat_{patientId}";


    public string GetMyId() => Context.UserIdentifier ?? "null";
}
