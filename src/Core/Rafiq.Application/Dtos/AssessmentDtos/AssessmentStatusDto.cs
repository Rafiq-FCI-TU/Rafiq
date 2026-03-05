using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.AssessmentDtos;
public class AssessmentStatusDto
{
    public int AssessmentId { get; set; }
    public AssessmentStatus Status { get; set; }
    public int TotalCategories { get; set; }
    public AssessmentCategoryDto? CurrentCategory { get; set; }
}