using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Hubs;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Services.Models;
using System.Security.Claims;

namespace Rafiq.API.Controllers;

/// <summary>
/// Handles direct messaging between specialists and families.
/// </summary>
[Authorize]
public class ChatController : BaseController
{
    private readonly IChatService _chatService;

    protected string CurrentUserId
      => User.FindFirstValue("uid")!;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    #region Send Message

    /// <summary>
    /// Sends a direct message to another user.
    /// Also pushes the message in real-time via SignalR to the receiver.
    /// </summary>
    /// <param name="body">The receiver ID and message content.</param>
    /// <returns>The saved message DTO.</returns>
    /// <response code="200">Message sent successfully.</response>
    /// <response code="400">Content is empty or sender equals receiver.</response>
    /// <response code="403">No specialist-family link between the two users.</response>
    /// <response code="404">Sender or receiver not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("send")]
    [ProducesResponseType(typeof(ChatMessageDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Send([FromBody] SendMessageBody body)
    {
        var claims = User.Claims.ToList(); 

        var result = await _chatService.SendAsync(new SendChatMessageRequest(CurrentUserId, body.ReceiverId, body.Content));
        return result.ToActionResult();
    }

    #endregion

    #region Get Conversations

    /// <summary>
    /// Returns all conversations for the authenticated user
    /// with the last message and unread count per conversation.
    /// </summary>
    /// <returns>List of conversations ordered by most recent message.</returns>
    /// <response code="200">Conversations returned successfully.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("conversations")]
    [ProducesResponseType(typeof(IReadOnlyList<ConversationDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetConversations()
    {
        var result = await _chatService.GetConversationsAsync(CurrentUserId);
        return result.ToActionResult();
    }

    #endregion

    #region Get History

    /// <summary>
    /// Returns paginated message history between the current user and another user.
    /// Messages are ordered by most recent first.
    /// </summary>
    /// <param name="otherUserId">The ID of the other participant.</param>
    /// <param name="pageNumber">Page number (default: 1).</param>
    /// <param name="pageSize">Page size between 1 and 100 (default: 30).</param>
    /// <returns>Paginated list of messages with metadata.</returns>
    /// <response code="200">History returned successfully.</response>
    /// <response code="403">No specialist-family link between the two users.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("history/{otherUserId}")]
    [ProducesResponseType(typeof(ChatHistoryDto), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetHistory(
        string otherUserId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 30)
    {
        var result = await _chatService.GetHistoryAsync(
            CurrentUserId, otherUserId, pageNumber, pageSize);
        return result.ToActionResult();
    }

    #endregion

    #region Mark As Read

    /// <summary>
    /// Marks all unread messages received from the specified user as read.
    /// </summary>
    /// <param name="otherUserId">The ID of the message sender.</param>
    /// <returns>Number of messages marked as read.</returns>
    /// <response code="200">Messages marked successfully.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("read/{otherUserId}")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> MarkAsRead(string otherUserId)
    {
        var result = await _chatService.MarkAsReadAsync(CurrentUserId, otherUserId);
        return result.ToActionResult();
    }

    #endregion

}


public sealed record SendMessageBody(string ReceiverId, string Content);