using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Dtos.AssessmentDtos;
public class AssessmentResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<AssessmentCategoryDto> Categories { get; set; } = [];
}
