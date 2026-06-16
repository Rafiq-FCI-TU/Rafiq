using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Dtos.CommunityDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities.Community;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Persistence.Data;

namespace Rafiq.Infrastructure.Services;

public class CommunityService : ICommunityService
{
    private readonly RafiqDbContext _context;

    public CommunityService(RafiqDbContext context)
    {
        _context = context;
    }

    // ───────────────────────────────────────────
    // Posts
    // ───────────────────────────────────────────

    public async Task<Result<PostResponseDto>> CreatePostAsync(string userId, CreatePostDto dto)
    {
        try
        {
            var post = new CommunityPost
            {
                Content = dto.Content,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Tags = NormalizeTags(dto.Tags)
                    .Select(t => new PostTag { Value = t })
                    .ToList()
            };

            _context.CommunityPosts.Add(post);
            await _context.SaveChangesAsync();

            // Reload with includes for the response
            var created = await GetPostWithIncludesQuery()
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            return Result.Success(MapToPostResponseDto(created!, userId));
        }
        catch (Exception ex)
        {
            return Result.Failure<PostResponseDto>(500, $"Failed to create post: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<PostResponseDto>>> GetAllPostsAsync(string? currentUserId)
    {
        try
        {
            var posts = await GetPostWithIncludesQuery()
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var result = posts.Select(p => MapToPostResponseDto(p, currentUserId)).ToList();
            return Result.Success<IReadOnlyList<PostResponseDto>>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<PostResponseDto>>(500, $"Failed to fetch posts: {ex.Message}");
        }
    }

    public async Task<Result<PostResponseDto>> GetPostByIdAsync(int id, string? currentUserId)
    {
        try
        {
            var post = await GetPostWithIncludesQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
                return Result.Failure<PostResponseDto>(404, "Post not found.");

            return Result.Success(MapToPostResponseDto(post, currentUserId));
        }
        catch (Exception ex)
        {
            return Result.Failure<PostResponseDto>(500, $"Failed to fetch post: {ex.Message}");
        }
    }

    public async Task<Result<PostResponseDto>> UpdatePostAsync(int id, string userId, UpdatePostDto dto)
    {
        try
        {
            var post = await _context.CommunityPosts.FindAsync(id);

            if (post is null)
                return Result.Failure<PostResponseDto>(404, "Post not found.");

            if (post.AppUserId != userId)
                return Result.Failure<PostResponseDto>(403, "You are not authorized to edit this post.");

            post.Content = dto.Content;

            // Replace tags
            var existingTags = await _context.PostTags
                .Where(t => t.CommunityPostId == id)
                .ToListAsync();
            _context.PostTags.RemoveRange(existingTags);

            var normalized = NormalizeTags(dto.Tags);
            if (normalized.Count > 0)
            {
                var newTags = normalized.Select(t => new PostTag
                {
                    Value = t,
                    CommunityPostId = id
                }).ToList();
                _context.PostTags.AddRange(newTags);
            }

            await _context.SaveChangesAsync();

            // Reload with includes for the response
            var updated = await GetPostWithIncludesQuery()
                .FirstOrDefaultAsync(p => p.Id == id);

            return Result.Success(MapToPostResponseDto(updated!, userId));
        }
        catch (Exception ex)
        {
            return Result.Failure<PostResponseDto>(500, $"Failed to update post: {ex.Message}");
        }
    }

    public async Task<Result> DeletePostAsync(int id, string userId)
    {
        try
        {
            var post = await _context.CommunityPosts.FindAsync(id);

            if (post is null)
                return Result.Failure(404, "Post not found.");

            if (post.AppUserId != userId)
                return Result.Failure(403, "You are not authorized to delete this post.");

            _context.CommunityPosts.Remove(post);
            await _context.SaveChangesAsync();

            return Result.Success("Post deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, $"Failed to delete post: {ex.Message}");
        }
    }

    // ───────────────────────────────────────────
    // Comments
    // ───────────────────────────────────────────

    public async Task<Result<CommentResponseDto>> CreateCommentAsync(int postId, string userId, CreateCommentDto dto)
    {
        try
        {
            var post = await _context.CommunityPosts.FindAsync(postId);

            if (post is null)
                return Result.Failure<CommentResponseDto>(404, "Post not found.");

            var comment = new Comment
            {
                Content = dto.Content,
                CommunityPostId = postId,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);

            // Maintain counter
            post.CommentsCount++;

            await _context.SaveChangesAsync();

            // Reload with includes for the response
            var created = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Reactions)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            return Result.Success(MapToCommentResponseDto(created!, userId));
        }
        catch (Exception ex)
        {
            return Result.Failure<CommentResponseDto>(500, $"Failed to create comment: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyList<CommentResponseDto>>> GetCommentsByPostIdAsync(int postId, string? currentUserId)
    {
        try
        {
            var postExists = await _context.CommunityPosts.AnyAsync(p => p.Id == postId);

            if (!postExists)
                return Result.Failure<IReadOnlyList<CommentResponseDto>>(404, "Post not found.");

            var comments = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Reactions)
                .AsNoTracking()
                .Where(c => c.CommunityPostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var result = comments.Select(c => MapToCommentResponseDto(c, currentUserId)).ToList();
            return Result.Success<IReadOnlyList<CommentResponseDto>>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<CommentResponseDto>>(500, $"Failed to fetch comments: {ex.Message}");
        }
    }

    public async Task<Result> DeleteCommentAsync(int id, string userId)
    {
        try
        {
            var comment = await _context.Comments
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return Result.Failure(404, "Comment not found.");

            if (comment.AppUserId != userId)
                return Result.Failure(403, "You are not authorized to delete this comment.");

            // Maintain counter
            comment.Post.CommentsCount = Math.Max(0, comment.Post.CommentsCount - 1);

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Result.Success("Comment deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, $"Failed to delete comment: {ex.Message}");
        }
    }

    // ───────────────────────────────────────────
    // Reactions
    // ───────────────────────────────────────────

    public async Task<Result<ReactionSummaryDto>> TogglePostReactionAsync(int postId, string userId, ToggleReactionDto dto)
    {
        try
        {
            var post = await _context.CommunityPosts.FindAsync(postId);

            if (post is null)
                return Result.Failure<ReactionSummaryDto>(404, "Post not found.");

            var existing = await _context.PostReactions
                .FirstOrDefaultAsync(r => r.CommunityPostId == postId && r.AppUserId == userId);

            if (existing is null)
            {
                // ADD new reaction
                _context.PostReactions.Add(new PostReaction
                {
                    CommunityPostId = postId,
                    AppUserId = userId,
                    Type = dto.Type
                });
                post.TotalReactionsCount++;
            }
            else if (existing.Type == dto.Type)
            {
                // REMOVE reaction (same type toggle)
                _context.PostReactions.Remove(existing);
                post.TotalReactionsCount = Math.Max(0, post.TotalReactionsCount - 1);
            }
            else
            {
                // UPDATE reaction type (different type)
                existing.Type = dto.Type;
            }

            await _context.SaveChangesAsync();

            var summary = await BuildPostReactionSummaryAsync(postId, userId);
            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ReactionSummaryDto>(500, $"Failed to toggle post reaction: {ex.Message}");
        }
    }

    public async Task<Result<ReactionSummaryDto>> ToggleCommentReactionAsync(int commentId, string userId, ToggleReactionDto dto)
    {
        try
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment is null)
                return Result.Failure<ReactionSummaryDto>(404, "Comment not found.");

            var existing = await _context.CommentReactions
                .FirstOrDefaultAsync(r => r.CommentId == commentId && r.AppUserId == userId);

            if (existing is null)
            {
                // ADD new reaction
                _context.CommentReactions.Add(new CommentReaction
                {
                    CommentId = commentId,
                    AppUserId = userId,
                    Type = dto.Type
                });
                comment.TotalReactionsCount++;
            }
            else if (existing.Type == dto.Type)
            {
                // REMOVE reaction (same type toggle)
                _context.CommentReactions.Remove(existing);
                comment.TotalReactionsCount = Math.Max(0, comment.TotalReactionsCount - 1);
            }
            else
            {
                // UPDATE reaction type (different type)
                existing.Type = dto.Type;
            }

            await _context.SaveChangesAsync();

            var summary = await BuildCommentReactionSummaryAsync(commentId, userId);
            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ReactionSummaryDto>(500, $"Failed to toggle comment reaction: {ex.Message}");
        }
    }

    public async Task<Result<ReactionSummaryDto>> GetPostReactionsAsync(int postId, string? currentUserId)
    {
        try
        {
            var postExists = await _context.CommunityPosts.AnyAsync(p => p.Id == postId);

            if (!postExists)
                return Result.Failure<ReactionSummaryDto>(404, "Post not found.");

            var summary = await BuildPostReactionSummaryAsync(postId, currentUserId);
            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ReactionSummaryDto>(500, $"Failed to fetch post reactions: {ex.Message}");
        }
    }

    public async Task<Result<ReactionSummaryDto>> GetCommentReactionsAsync(int commentId, string? currentUserId)
    {
        try
        {
            var commentExists = await _context.Comments.AnyAsync(c => c.Id == commentId);

            if (!commentExists)
                return Result.Failure<ReactionSummaryDto>(404, "Comment not found.");

            var summary = await BuildCommentReactionSummaryAsync(commentId, currentUserId);
            return Result.Success(summary);
        }
        catch (Exception ex)
        {
            return Result.Failure<ReactionSummaryDto>(500, $"Failed to fetch comment reactions: {ex.Message}");
        }
    }

    // ───────────────────────────────────────────
    // Private Helpers
    // ───────────────────────────────────────────

    private IQueryable<CommunityPost> GetPostWithIncludesQuery()
    {
        return _context.CommunityPosts
            .Include(p => p.Author)
            .Include(p => p.Tags)
            .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
            .Include(p => p.Comments)
                .ThenInclude(c => c.Reactions)
            .Include(p => p.Reactions);
    }

    private static PostResponseDto MapToPostResponseDto(CommunityPost post, string? currentUserId)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            PostedAgo = post.PostedAgo,
            Tags = post.Tags?.Select(t => t.Value).ToList(),
            TotalReactionsCount = post.TotalReactionsCount,
            CommentsCount = post.CommentsCount,
            Author = new PostAuthorDto
            {
                Id = post.Author.Id,
                FirstName = post.Author.FirstName,
                LastName = post.Author.LastName
            },
            ReactionSummary = BuildReactionSummary(
                post.Reactions.Select(r => r.Type).ToList(),
                currentUserId != null
                    ? post.Reactions.FirstOrDefault(r => r.AppUserId == currentUserId)?.Type
                    : null
            ),
            Comments = post.Comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => MapToCommentResponseDto(c, currentUserId))
                .ToList()
        };
    }

    private static CommentResponseDto MapToCommentResponseDto(Comment comment, string? currentUserId)
    {
        return new CommentResponseDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            PostedAgo = comment.PostedAgo,
            TotalReactionsCount = comment.TotalReactionsCount,
            Author = new PostAuthorDto
            {
                Id = comment.Author.Id,
                FirstName = comment.Author.FirstName,
                LastName = comment.Author.LastName
            },
            ReactionSummary = BuildReactionSummary(
                comment.Reactions.Select(r => r.Type).ToList(),
                currentUserId != null
                    ? comment.Reactions.FirstOrDefault(r => r.AppUserId == currentUserId)?.Type
                    : null
            )
        };
    }

    private static ReactionSummaryDto BuildReactionSummary(List<ReactionType> reactions, ReactionType? userReaction)
    {
        var grouped = reactions
            .GroupBy(r => r)
            .ToDictionary(g => g.Key, g => g.Count());

        return new ReactionSummaryDto
        {
            Total = reactions.Count,
            Types = new Dictionary<string, int>
            {
                ["like"] = grouped.GetValueOrDefault(ReactionType.Like, 0),
                ["love"] = grouped.GetValueOrDefault(ReactionType.Love, 0),
                ["haha"] = grouped.GetValueOrDefault(ReactionType.Haha, 0),
                ["wow"] = grouped.GetValueOrDefault(ReactionType.Wow, 0),
                ["sad"] = grouped.GetValueOrDefault(ReactionType.Sad, 0),
                ["angry"] = grouped.GetValueOrDefault(ReactionType.Angry, 0)
            },
            UserReaction = userReaction?.ToString().ToLowerInvariant()
        };
    }

    private async Task<ReactionSummaryDto> BuildPostReactionSummaryAsync(int postId, string? currentUserId)
    {
        var reactions = await _context.PostReactions
            .AsNoTracking()
            .Where(r => r.CommunityPostId == postId)
            .ToListAsync();

        var userReaction = currentUserId != null
            ? reactions.FirstOrDefault(r => r.AppUserId == currentUserId)?.Type
            : null;

        return BuildReactionSummary(
            reactions.Select(r => r.Type).ToList(),
            userReaction
        );
    }

    private async Task<ReactionSummaryDto> BuildCommentReactionSummaryAsync(int commentId, string? currentUserId)
    {
        var reactions = await _context.CommentReactions
            .AsNoTracking()
            .Where(r => r.CommentId == commentId)
            .ToListAsync();

        var userReaction = currentUserId != null
            ? reactions.FirstOrDefault(r => r.AppUserId == currentUserId)?.Type
            : null;

        return BuildReactionSummary(
            reactions.Select(r => r.Type).ToList(),
            userReaction
        );
    }

    private static List<string> NormalizeTags(List<string>? tags)
    {
        if (tags is null || tags.Count == 0)
            return new List<string>();

        return tags
            .Select(t => t.Trim().ToLowerInvariant())
            .Where(t => !string.IsNullOrEmpty(t))
            .Distinct()
            .ToList();
    }
}
