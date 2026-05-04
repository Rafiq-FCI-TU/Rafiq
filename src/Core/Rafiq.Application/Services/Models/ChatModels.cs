using Rafiq.Application.Hubs;

namespace Rafiq.Application.Services.Models;

// ─── Requests ────────────────────────────────────────────────────────────────

public sealed record SendChatMessageRequest(
    string SenderId,
    string ReceiverId,
    string Content);

public sealed record GetHistoryRequest(
    string CurrentUserId,
    string OtherUserId,
    int PageNumber = 1,
    int PageSize = 30);

// ─── Responses ───────────────────────────────────────────────────────────────

public sealed record ChatHistoryDto(
    IReadOnlyList<ChatMessageDto> Messages,
    int PageNumber,
    int PageSize,
    int TotalCount, string PartnerName)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}

public sealed record ConversationDto(
    string PartnerId,
    string PartnerName,
    ChatMessageDto? LastMessage,
    int UnreadCount);
