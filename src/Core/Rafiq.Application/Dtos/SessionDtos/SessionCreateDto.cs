using Microsoft.AspNetCore.Http;

namespace Rafiq.Application.Dtos.SessionDtos;
public class SessionCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile Media { get; set; } = null!;
    public IFormFile? Thumbnail { get; set; } = null;
    public string Duration { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public int Score { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.Now;

    public string SpecialistProfileId { get; set; } = string.Empty;
}