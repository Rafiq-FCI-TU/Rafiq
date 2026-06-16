using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.CommunityDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/community")]
public class CommunityController : BaseController
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    // ───────────────────────────────────────────
    // Posts
    // ───────────────────────────────────────────

    [Authorize]
    [HttpPost("posts")]
    [ProducesResponseType(typeof(PostResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.CreatePostAsync(userId, dto)).ToActionResult();
    }

    [HttpGet("posts")]
    [ProducesResponseType(typeof(IReadOnlyList<PostResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllPosts()
        => (await _communityService.GetAllPostsAsync(GetUserId())).ToActionResult();

    [HttpGet("posts/{id:int}")]
    [ProducesResponseType(typeof(PostResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPostById(int id)
        => (await _communityService.GetPostByIdAsync(id, GetUserId())).ToActionResult();

    [Authorize]
    [HttpPut("posts/{id:int}")]
    [ProducesResponseType(typeof(PostResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.UpdatePostAsync(id, userId, dto)).ToActionResult();
    }

    [Authorize]
    [HttpDelete("posts/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeletePost(int id)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.DeletePostAsync(id, userId)).ToActionResult();
    }

    // ───────────────────────────────────────────
    // Comments
    // ───────────────────────────────────────────

    [Authorize]
    [HttpPost("posts/{postId:int}/comments")]
    [ProducesResponseType(typeof(CommentResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] CreateCommentDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.CreateCommentAsync(postId, userId, dto)).ToActionResult();
    }

    [HttpGet("posts/{postId:int}/comments")]
    [ProducesResponseType(typeof(IReadOnlyList<CommentResponseDto>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetCommentsByPostId(int postId)
        => (await _communityService.GetCommentsByPostIdAsync(postId, GetUserId())).ToActionResult();

    [Authorize]
    [HttpDelete("comments/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.DeleteCommentAsync(id, userId)).ToActionResult();
    }

    // ───────────────────────────────────────────
    // Reactions
    // ───────────────────────────────────────────

    [Authorize]
    [HttpPost("posts/{postId:int}/reactions/toggle")]
    [ProducesResponseType(typeof(ReactionSummaryDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> TogglePostReaction(int postId, [FromBody] ToggleReactionDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.TogglePostReactionAsync(postId, userId, dto)).ToActionResult();
    }

    [Authorize]
    [HttpPost("comments/{commentId:int}/reactions/toggle")]
    [ProducesResponseType(typeof(ReactionSummaryDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ToggleCommentReaction(int commentId, [FromBody] ToggleReactionDto dto)
    {
        var userId = GetUserId();
        if (userId is null)
            return Result.Unauthorized("User not authenticated.").ToActionResult();

        return (await _communityService.ToggleCommentReactionAsync(commentId, userId, dto)).ToActionResult();
    }

    [HttpGet("posts/{postId:int}/reactions")]
    [ProducesResponseType(typeof(ReactionSummaryDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPostReactions(int postId)
        => (await _communityService.GetPostReactionsAsync(postId, GetUserId())).ToActionResult();

    [HttpGet("comments/{commentId:int}/reactions")]
    [ProducesResponseType(typeof(ReactionSummaryDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetCommentReactions(int commentId)
        => (await _communityService.GetCommentReactionsAsync(commentId, GetUserId())).ToActionResult();

    // ───────────────────────────────────────────
    // Helpers
    // ───────────────────────────────────────────

    private string? GetUserId()
        => User.FindFirstValue("uid")
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
}
