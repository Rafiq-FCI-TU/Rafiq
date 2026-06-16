using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.ResourceDtos;

public class CreateResourceDto : IValidatableObject
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Link is required.")]
    [MaxLength(2048, ErrorMessage = "Link must not exceed 2048 characters.")]
    [Url(ErrorMessage = "Link must be a valid URL.")]
    public string Link { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "Description must not exceed 2000 characters.")]
    public string? Description { get; set; }

    public List<string>? Tags { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => ValidateTags(Tags);

    protected static IEnumerable<ValidationResult> ValidateTags(IEnumerable<string>? tags)
    {
        if (tags is null)
            yield break;

        foreach (var tag in tags)
        {
            if (string.IsNullOrWhiteSpace(tag))
                continue;

            if (tag.Trim().Length > 100)
            {
                yield return new ValidationResult(
                    "Each tag must not exceed 100 characters.",
                    new[] { nameof(Tags) });
            }
        }
    }
}

public class UpdateResourceDto : CreateResourceDto;

public class ResourceAuthorDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
}

public class ResourceResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public ResourceAuthorDto Author { get; set; } = null!;
}
