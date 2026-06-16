using Rafiq.Application.Hubs;
using Rafiq.Application.Services.Models;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IChatService
{
    /// <summary>
    /// Sends a direct message. Validates access before persisting.
    /// </summary>
    Task<Result<ChatMessageDto>> SendAsync(
        SendChatMessageRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Paginated message history between two users (newest first).
    /// </summary>
    Task<Result<ChatHistoryDto>> GetHistoryAsync(
        string currentUserId,
        string otherUserId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// All conversations for the current user with last message + unread count.
    /// </summary>
    Task<Result<IReadOnlyList<ConversationDto>>> GetConversationsAsync(
        string currentUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks all unread messages from otherUserId as read.
    /// Returns the number of messages marked.
    /// </summary>
    Task<Result<int>> MarkAsReadAsync(
        string currentUserId,
        string otherUserId,
        CancellationToken cancellationToken = default);
}