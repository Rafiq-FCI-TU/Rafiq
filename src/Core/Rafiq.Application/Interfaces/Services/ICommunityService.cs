using Rafiq.Application.Dtos.CommunityDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface ICommunityService
{
    // Posts
    Task<Result<PostResponseDto>> CreatePostAsync(string userId, CreatePostDto dto);
    Task<Result<IReadOnlyList<PostResponseDto>>> GetAllPostsAsync(string? currentUserId);
    Task<Result<PostResponseDto>> GetPostByIdAsync(int id, string? currentUserId);
    Task<Result<PostResponseDto>> UpdatePostAsync(int id, string userId, UpdatePostDto dto);
    Task<Result> DeletePostAsync(int id, string userId);

    // Comments
    Task<Result<CommentResponseDto>> CreateCommentAsync(int postId, string userId, CreateCommentDto dto);
    Task<Result<IReadOnlyList<CommentResponseDto>>> GetCommentsByPostIdAsync(int postId, string? currentUserId);
    Task<Result> DeleteCommentAsync(int id, string userId);

    // Reactions
    Task<Result<ReactionSummaryDto>> TogglePostReactionAsync(int postId, string userId, ToggleReactionDto dto);
    Task<Result<ReactionSummaryDto>> ToggleCommentReactionAsync(int commentId, string userId, ToggleReactionDto dto);
    Task<Result<ReactionSummaryDto>> GetPostReactionsAsync(int postId, string? currentUserId);
    Task<Result<ReactionSummaryDto>> GetCommentReactionsAsync(int commentId, string? currentUserId);
}
