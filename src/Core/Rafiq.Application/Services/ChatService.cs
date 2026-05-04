using AutoMapper;
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
    private readonly IMapper _mapper;

    public ChatService(IApplicationDbContext db, IHubContext<AppHub> hub, IMapper mapper)
    {
        _db     = db;
        _hub    = hub;
        _mapper = mapper;
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

        if (request.SenderId == request.ReceiverId)
            return Result.Failure<ChatMessageDto>(400, "Cannot send a message to yourself.");

        var sender = await _db.Users
            .FirstOrDefaultAsync(u =>
                u.Id       == request.SenderId ||
                u.UserName == request.SenderId ||
                u.Email    == request.SenderId,
                cancellationToken);

        if (sender is null)
            return Result.Failure<ChatMessageDto>(404, "Sender not found.");

        var receiver = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == request.ReceiverId, cancellationToken);

        if (receiver is null)
            return Result.Failure<ChatMessageDto>(404, "Receiver not found.");

        if (!await HasChatAccessAsync(request.SenderId, request.ReceiverId, cancellationToken))
            return Result.Failure<ChatMessageDto>(403, "Messaging is only allowed between linked specialists and families.");

        var message = new ChatMessage
        {
            SenderId   = sender.Id,        
            ReceiverId = request.ReceiverId,
            Content    = request.Content.Trim(),
            SentAt     = DateTime.UtcNow,
            IsRead     = false,
            Sender     = sender,           
            Receiver   = receiver          
        };

        _db.ChatMessages.Add(message);
        await _db.SaveChangesAsync(cancellationToken);

       
        var dto = _mapper.Map<ChatMessageDto>(message);

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

        var baseQuery = _db.ChatMessages
            .Where(m =>
                (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                (m.SenderId == otherUserId   && m.ReceiverId == currentUserId));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var messages = await baseQuery
            .AsNoTracking()
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .OrderByDescending(m => m.SentAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var messageDtos = _mapper.Map<List<ChatMessageDto>>(messages);
        var parterName = await _db.Users
            .Where(u => u.Id == otherUserId)
            .Select(u => $"{u.FirstName} {u.LastName}".Trim())
            .FirstOrDefaultAsync(cancellationToken);    

        return Result.Success(new ChatHistoryDto(messageDtos, pageNumber, pageSize, totalCount, parterName));
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Conversations list
    // ─────────────────────────────────────────────────────────────────────────

    public async Task<Result<IReadOnlyList<ConversationDto>>> GetConversationsAsync(
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        var partnerIds = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (partnerIds.Count == 0)
            return Result.Success<IReadOnlyList<ConversationDto>>(Array.Empty<ConversationDto>());

        var allUserIds = partnerIds.Append(currentUserId).Distinct().ToList();
        var nameMap = await _db.Users
            .Where(u => allUserIds.Contains(u.Id))
            .ToDictionaryAsync(
                u => u.Id,
                u => $"{u.FirstName} {u.LastName}".Trim(),
                cancellationToken);

        var latestMessageIds = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
            .Select(g => g.OrderByDescending(m => m.SentAt).First().Id)
            .ToListAsync(cancellationToken);

        var latestMessages = await _db.ChatMessages
            .AsNoTracking()
            .Include(m => m.Sender)      
            .Include(m => m.Receiver)    
            .Where(m => latestMessageIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        var latestByPartner = latestMessages
            .ToDictionary(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId);

        var unreadCounts = await _db.ChatMessages
            .AsNoTracking()
            .Where(m => m.ReceiverId == currentUserId && !m.IsRead && partnerIds.Contains(m.SenderId))
            .GroupBy(m => m.SenderId)
            .Select(g => new { PartnerId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PartnerId, x => x.Count, cancellationToken);

        var conversations = partnerIds
            .Select(partnerId =>
            {
                latestByPartner.TryGetValue(partnerId, out var last);
                var unread = unreadCounts.GetValueOrDefault(partnerId, 0);

                var lastDto = last is null ? null : _mapper.Map<ChatMessageDto>(last);

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

    private async Task<bool> HasChatAccessAsync(
        string senderId,
        string receiverId,
        CancellationToken cancellationToken)
    {
        var senderIsSpecialist = await _db.SpecialistProfiles
            .AnyAsync(u => u.Id == senderId, cancellationToken);

        if (senderIsSpecialist)
        {
            return await _db.SpecialistPatients
                .Include(sp => sp.Patient)
                .AnyAsync(sp =>
                    sp.SpecialistId == senderId &&
                    sp.Patient.FamilyProfileId == receiverId,
                    cancellationToken);
        }

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
}