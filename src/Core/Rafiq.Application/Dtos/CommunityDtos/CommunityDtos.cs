using System.ComponentModel.DataAnnotations;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Application.Dtos.CommunityDtos;

// ───────────────────────────────────────────
// Request DTOs
// ───────────────────────────────────────────

public class CreatePostDto
{
    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Content must not exceed 5000 characters.")]
    public string Content { get; set; } = string.Empty;

    public List<string>? Tags { get; set; }
}

public class UpdatePostDto
{
    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Content must not exceed 5000 characters.")]
    public string Content { get; set; } = string.Empty;

    public List<string>? Tags { get; set; }
}

public class CreateCommentDto
{
    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(2000, ErrorMessage = "Content must not exceed 2000 characters.")]
    public string Content { get; set; } = string.Empty;
}

public class ToggleReactionDto
{
    [Required(ErrorMessage = "Reaction type is required.")]
    [EnumDataType(typeof(ReactionType), ErrorMessage = "Invalid reaction type.")]
    public ReactionType Type { get; set; }
}

// ───────────────────────────────────────────
// Response DTOs
// ───────────────────────────────────────────

public class PostAuthorDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class ReactionSummaryDto
{
    public int Total { get; set; }
    public Dictionary<string, int> Types { get; set; } = new()
    {
        ["like"] = 0,
        ["love"] = 0,
        ["haha"] = 0,
        ["wow"] = 0,
        ["sad"] = 0,
        ["angry"] = 0
    };
    public string? UserReaction { get; set; }
}

public class CommentResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PostedAgo { get; set; } = string.Empty;
    public int TotalReactionsCount { get; set; }
    public PostAuthorDto Author { get; set; } = null!;
    public ReactionSummaryDto ReactionSummary { get; set; } = new();
}

public class PostResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PostedAgo { get; set; } = string.Empty;
    public List<string>? Tags { get; set; }
    public int TotalReactionsCount { get; set; }
    public int CommentsCount { get; set; }
    public PostAuthorDto Author { get; set; } = null!;
    public ReactionSummaryDto ReactionSummary { get; set; } = new();
    public List<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();
}
