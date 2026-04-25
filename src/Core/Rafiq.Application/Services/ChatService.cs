using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Hubs;
using Rafiq.Application.Interfaces;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Services.Models;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Services;

public class ChatService : IChatService
{
    private readonly IApplicationDbContext _db;
    private readonly IHubContext<AppHub> _hub;

    public ChatService(IApplicationDbContext db, IHubContext<AppHub> hub)
    {
        _db = db;
        _hub = hub;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Send
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<Result<ChatMessageDto>> SendAsync(
        SendChatMessageRequest request,
        CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrWhiteSpace(request.Content))
            return Result.Failure<ChatMessageDto>(400, "Message content is required.");

        var sender = await _db.Users
            .FirstOrDefaultAsync(u =>
                u.Id == request.SenderId ||
                u.UserName == request.SenderId ||
                u.Email == request.SenderId,
                cancellationToken);

        if (sender is null)
            return Result.Failure<ChatMessageDto>(404, "Sender not found.");

        if (string.IsNullOrWhiteSpace(request.Content))
            return Result.Failure<ChatMessageDto>(400, "Message content is required.");

        if (request.SenderId == request.ReceiverId)
            return Result.Failure<ChatMessageDto>(400, "Cannot send a message to yourself.");

        // Load both users in one query
        var users = await _db.Users
            .Where(u => u.Id == request.SenderId || u.Id == request.ReceiverId)
            .ToListAsync(cancellationToken);

        var receiver = users.FirstOrDefault(u => u.Id == request.ReceiverId);

        if (sender is null)
            return Result.Failure<ChatMessageDto>(404, "Sender not found.");

        if (receiver is null)
            return Result.Failure<ChatMessageDto>(404, "Receiver not found.");

        if (!await HasChatAccessAsync(request.SenderId, request.ReceiverId, cancellationToken))
            return Result.Failure<ChatMessageDto>(403, "Messaging is only allowed between linked specialists and families.");

        var message = new ChatMessage
        {
            SenderId   = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content    = request.Content.Trim(),
            SentAt     = DateTime.UtcNow,
            IsRead     = false
        };

        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync(cancellationToken);

        var dto = MapToDto(message, sender);

        // Push to receiver via SignalR (fire-and-forget — don't block on delivery)
        _ = _hub.Clients.User(request.ReceiverId)
            .SendAsync(HubEvents.NewMessage, dto, cancellationToken);

        return Result.Success(dto);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // History
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<Result<ChatHistoryDto>> GetHistoryAsync(
        string currentUserId,
        string otherUserId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize   = Math.Clamp(pageSize, 1, 100);

        if (!await HasChatAccessAsync(currentUserId, otherUserId, cancellationToken))
            return Result.Failure<ChatHistoryDto>(403, "Messaging is only allowed between linked specialists and families.");

        // Base filter — reused for count and fetch
        var baseQuery = _db.ChatMessages
            .Where(m =>
                (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId   && m.ReceiverId == currentUserId));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        // Resolve display names in one query
        var nameMap = await _db.Users
            .Where(u => u.Id == currentUserId || u.Id == otherUserId)
            .ToDictionaryAsync(
                u => u.Id,
                u => $"{u.FirstName} {u.LastName}".Trim(),
                cancellationToken);

        var messages = await baseQuery
            .AsNoTracking()
            .OrderByDescending(m => m.SentAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new ChatMessageDto(
                m.Id,
                m.SenderId,
                nameMap.GetValueOrDefault(m.SenderId, m.SenderId),
                m.ReceiverId,
                m.Content,
                m.SentAt,
                m.IsRead,
                m.ReadAt))
            .ToListAsync(cancellationToken);

        return Result.Success(new ChatHistoryDto(messages, pageNumber, pageSize, totalCount));
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Conversations list  (fixed N+1)
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<Result<IReadOnlyList<ConversationDto>>> GetConversationsAsync(
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        // Step 1 — find all partner IDs
        var partnerIds = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (partnerIds.Count == 0)
            return Result.Success<IReadOnlyList<ConversationDto>>(Array.Empty<ConversationDto>());

        // Step 2 — resolve all display names in ONE query
        var allUserIds = partnerIds.Append(currentUserId).Distinct().ToList();
        var nameMap = await _db.Users
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(
                u => u.Id,
                u => $"{u.FirstName} {u.LastName}".Trim(),
                cancellationToken);

        // Step 3 — fetch the latest message per conversation in ONE query using
        //          a subquery so EF can translate it to SQL MAX + JOIN
        var latestMessageIds = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Select(g => g.OrderByDescending(m => m.SentAt).First().Id)
            .ToListAsync(cancellationToken);

        var latestMessages = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => latestMessageIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        var latestByPartner = latestMessages
            .ToDictionary(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId);

        // Step 4 — unread counts per partner in ONE query
        var unreadCounts = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.ReceiverId == currentUserId && !m.IsRead && partnerIds.Contains(m.SenderId))
            .GroupBy(m => m.SenderId)
            .Select(g => new { PartnerId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PartnerId, x => x.Count, cancellationToken);

        // Step 5 — assemble result
        var conversations = partnerIds
            .Select(partnerId =>
            {
                latestByPartner.TryGetValue(partnerId, out var last);
                var unread = unreadCounts.GetValueOrDefault(partnerId, 0);

                ChatMessageDto? lastDto = last is null ? null : new ChatMessageDto(
                    last.Id,
                    last.SenderId,
                    nameMap.GetValueOrDefault(last.SenderId, last.SenderId),
                    last.ReceiverId,
                    last.Content,
                    last.SentAt,
                    last.IsRead,
                    last.ReadAt);

                return new ConversationDto(
                    partnerId,
                    nameMap.GetValueOrDefault(partnerId, partnerId),
                    lastDto,
                    unread);
            })
            .OrderByDescending(c => c.LastMessage?.SentAt ?? DateTime.MinValue)
            .ToList();

        return Result.Success<IReadOnlyList<ConversationDto>>(conversations);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Mark as read
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<Result<int>> MarkAsReadAsync(
        string currentUserId,
        string otherUserId,
        CancellationToken cancellationToken = default)
    {
        var messages = await _db.ChatMessages
            .Where(m =>
                m.SenderId   == otherUserId   &&
                m.ReceiverId == currentUserId &&
                !m.IsRead)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return Result.Success(0);

        var now = DateTime.UtcNow;
        foreach (var msg in messages)
        {
            msg.IsRead = true;
            msg.ReadAt = now;
        }

        await _db.SaveChangesAsync(cancellationToken);
        return Result.Success(messages.Count);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Checks access in 2 queries instead of 4:
    /// one to identify sender role, one to verify the specialist-family link.
    /// </summary>
    private async Task<bool> HasChatAccessAsync(
        string senderId,
        string receiverId,
        CancellationToken cancellationToken)
    {
        var senderIsSpecialist = await _db.SpecialistProfiles
            .AnyAsync(u => u.Id == senderId, cancellationToken);

        // Specialist → Family
        if (senderIsSpecialist)
        {
            return await _db.SpecialistPatients
                .Include(sp => sp.Patient)
                .AnyAsync(sp =>
                    sp.SpecialistId == senderId &&
                    sp.Patient.FamilyProfileId == receiverId,
                    cancellationToken);
        }

        // Family → Specialist (sender must be a family member)
        var senderIsFamily = await _db.FamilyProfiles
            .AnyAsync(u => u.Id == senderId, cancellationToken);

        if (!senderIsFamily)
            return false;

        return await _db.SpecialistPatients
            .Include(sp => sp.Patient)
            .AnyAsync(sp =>
                sp.SpecialistId == receiverId &&
                sp.Patient.FamilyProfileId == senderId,
                cancellationToken);
    }

    private static ChatMessageDto MapToDto(ChatMessage message, ApplicationUser sender)
        => new(
            message.Id,
            message.SenderId,
            $"{sender.FirstName} {sender.LastName}".Trim(),
            message.ReceiverId,
            message.Content,
            message.SentAt,
            message.IsRead,
            message.ReadAt);
}
