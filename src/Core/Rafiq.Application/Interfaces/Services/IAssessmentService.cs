using Rafiq.Application.Dtos.IQDtos;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;
public interface IAssessmentService
{
    Task<Result<AssessmentResponseDto>> CreateAssessmentForPatientAsync(int patientId);

    Task<Result<AssessmentStatusDto>> GetAssessmentStatusAsync(int assessmentId);

    Task<Result<AssessmentStatusDto>> SubmitAnswerAsync(SubmitAnswerDto dto);
}


public class AssessmentStatusDto
{
    public int AssessmentId { get; set; }
    public AssessmentStatus Status { get; set; }
    public int TotalCategories { get; set; }
    public int CompletedCategories { get; set; }
    public AssessmentCategoryDto2? CurrentCategory { get; set; }
}

//public class AssessmentCategoryDto
//{
//    public int AssessmentCategoryId { get; set; }
//    public string CategoryName { get; set; } = string.Empty;
//    public string CategoryDescription { get; set; } = string.Empty;
//    public int CurrentQuestion { get; set; }
//    public int TotalQuestions { get; set; }
//    public QuestionDto? Question { get; set; } // السؤال الحالي بس
//}

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public AnswerType AnswerType { get; set; }
    public List<AnswerOptionDto> Options { get; set; } = [];
}

public class AnswerOptionDto
{
    public int Value { get; set; }
    public string Label { get; set; } = string.Empty;
}

// Request: إجابة سؤال
public class SubmitAnswerDto
{
    public int AssessmentId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerValue { get; set; }
}

public class AssessmentResponseDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Status { get; set; }
    public List<AssessmentCategoryDto> Categories { get; set; }
}

public class AssessmentCategoryDto
{
    public int CategoryId { get; set; }
    public int QuestionsNumber { get; set; }
    public int CurrentQuestion { get; set; }
    public int Score { get; set; }
}

public class AssessmentCategoryDto2
{
    public int AssessmentCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public int CurrentQuestion { get; set; }
    public int TotalQuestions { get; set; }
    public QuestionDto? Question { get; set; } // السؤال الحالي بس
}