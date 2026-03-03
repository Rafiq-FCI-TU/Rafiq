using Rafiq.Application.Dtos.IQDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;
using AssessmentCategory = Rafiq.Domain.Entities.AssessmentCategory;
using QuestionDto = Rafiq.Application.Interfaces.Services.QuestionDto;

namespace Rafiq.Infrastructure.Services;
public class AssessmentService(IUnitOfWork unitOfWork) : IAssessmentService
{
    public async Task<Result<AssessmentResponseDto>> CreateAssessmentForPatientAsync(int patientId)
    {
        try
        {
            var spec = new BaseSpecifications<Category>();

            spec.AddIncludeWithThenInclude(a => a.Questions);

            var categories = await unitOfWork.Repository<Category>().GetAllAsync(spec);

            var assessment = new Assessment
            {
                PatientId = patientId,
                Status = AssessmentStatus.InProgress,
                Categories = categories.Select(c => new AssessmentCategory
                {
                    CategoryId = c.Id,
                    QuestionsNumber = c.Questions.Count > 0 ? c.QuestionsNumber : 10,
                    CurrentQuestion = 1,
                    Score = 0
                }).ToList()
            };

            await unitOfWork.Repository<Assessment>().AddAsync(assessment);

            await unitOfWork.CompleteAsync();

            var response = new AssessmentResponseDto
            {
                Id = assessment.Id,
                PatientId = assessment.PatientId,
                Status = assessment.Status.ToString(),
                Categories = assessment.Categories.Select(c => new AssessmentCategoryDto
                {
                    CategoryId = c.CategoryId,
                    QuestionsNumber = c.QuestionsNumber,
                    CurrentQuestion = c.CurrentQuestion,
                    Score = c.Score
                }).ToList()
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Result.Failure<AssessmentResponseDto>(400, ex.ToString());
        }
    }

    public async Task<Result<AssessmentStatusDto>> GetAssessmentStatusAsync(int assessmentId)
    {
        var spec = new BaseSpecifications<Assessment>
        {
            WhereCriteria = a => a.Id == assessmentId
        };

        spec.AddIncludeWithThenInclude(
            a => a.Categories,
            "Category",
            "Category.Questions"
        );

        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(spec);

        if (assessment == null)
        {
            throw new Exception("Assessment not found");
        }

        var totalCategories = assessment.Categories.Count;

        // أول category لسه مخلصتش (CurrentQuestion <= QuestionsNumber)
        var currentCategory = assessment.Categories
            .OrderBy(ac => ac.CategoryId)
            .FirstOrDefault(ac => ac.CurrentQuestion <= ac.QuestionsNumber);

        var completedCategories = assessment.Categories
            .Count(ac => ac.CurrentQuestion > ac.QuestionsNumber);

        AssessmentCategoryDto2? currentCategoryDto = null;

        if (currentCategory != null)
        {
            var question = currentCategory.Category.Questions
                .FirstOrDefault(q => q.Order == currentCategory.CurrentQuestion);

            currentCategoryDto = new AssessmentCategoryDto2
            {
                AssessmentCategoryId = currentCategory.Id,
                CategoryName = currentCategory.Category.Name,
                CategoryDescription = currentCategory.Category.Description,
                CurrentQuestion = currentCategory.CurrentQuestion,
                TotalQuestions = currentCategory.QuestionsNumber,
                Question = question == null ? null : new QuestionDto
                {
                    QuestionId = question.Id,
                    Text = question.Text,
                    AnswerType = question.AnswerType,
                    Options = GetOptionsForAnswerType(question.AnswerType)
                }
            };
        }

        return Result.Success(new AssessmentStatusDto
        {
            AssessmentId = assessmentId,
            Status = assessment.Status,
            TotalCategories = totalCategories,
            CurrentCategory = currentCategoryDto
        });
    }

    public async Task<Result<AssessmentStatusDto>> SubmitAnswerAsync(SubmitAnswerDto dto)
    {
        var spec = new BaseSpecifications<Assessment>
        {
            WhereCriteria = a => a.Id == dto.AssessmentId
        };

        spec.AddIncludeWithThenInclude(
            a => a.Categories,
            "Category",
            "Category.Questions"
        );

        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(spec);

        if(assessment == null)
            return Result.Failure<AssessmentStatusDto>(404, "Assessment not found");

        if (assessment.Status == AssessmentStatus.Completed)
            return Result.Failure<AssessmentStatusDto>(400, "Assessment already completed");

        var question = await unitOfWork.Repository<Question>().GetEntityAsync(dto.QuestionId);   

        if(question == null)
            return Result.Failure<AssessmentStatusDto>(404, "Question not found");

        var assessmentAnswerSpec = new BaseSpecifications<AssessmentAnswer>
        {
            WhereCriteria = a =>
                a.AssessmentId == dto.AssessmentId &&
                a.QuestionId == dto.QuestionId
        };

        // تأكد مش جاوب قبل كدا
        var existingAnswer = await unitOfWork.Repository<AssessmentAnswer>().GetEntityAsync(assessmentAnswerSpec);

        if (existingAnswer != null)
            return Result.Failure<AssessmentStatusDto>(400, "Question already answered");

        // احفظ الإجابة
        var answer = new AssessmentAnswer
        {
            AssessmentId = dto.AssessmentId,
            QuestionId = dto.QuestionId,
            AnswerValue = dto.AnswerValue
        };

        await unitOfWork.Repository<AssessmentAnswer>().AddAsync(answer);

        // حدّث الـ AssessmentCategory
        var assessmentCategory = assessment.Categories
            .FirstOrDefault(ac => ac.CategoryId == question.CategoryId);

        assessmentCategory.Score += dto.AnswerValue;
        assessmentCategory.CurrentQuestion++;

        // لو خلصت الـ Assessment كلها
        var allDone = assessment.Categories
            .All(ac => ac.CurrentQuestion > ac.QuestionsNumber);

        if (allDone)
            assessment.Status = AssessmentStatus.Completed;

        await unitOfWork.CompleteAsync();

        return await GetAssessmentStatusAsync(dto.AssessmentId);
    }

    private static List<AnswerOptionDto> GetOptionsForAnswerType(AnswerType answerType)
    {
        return answerType switch
        {
            AnswerType.Frequency => new List<AnswerOptionDto>
            {
                new() { Value = 0, Label = "Never" },
                new() { Value = 1, Label = "Rarely" },
                new() { Value = 2, Label = "Sometimes" },
                new() { Value = 3, Label = "Often" },
                new() { Value = 4, Label = "Always" }
            },
            AnswerType.DifficultyLevel => new List<AnswerOptionDto>
            {
                new() { Value = 0, Label = "No difficulty" },
                new() { Value = 1, Label = "Mild" },
                new() { Value = 2, Label = "Moderate" },
                new() { Value = 3, Label = "Severe" },
                new() { Value = 4, Label = "Cannot do" }
            },
            _ => []
        };
    }

















    public async Task<Result<Assessment?>> GetAssessmentByPatientIdAsync(int patientId)
    {
        var specification = new AssessmentWithDetailsSpec(patientId);

        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(specification);

        return Result.Success(assessment);
    }

    //public Task<AssessmentDto> StartAssessmentAsync(int patientId)
    //{
    //    throw new NotImplementedException();
    //}

    public Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync()
    {
        throw new NotImplementedException();
    }


    //public async Task<AssessmentDto> StartAssessmentAsync(int patientId)
    //{
    //    ///TODO FindSpeAsync(a => a.PatientId == patientId && a.Status == AssessmentStatus.InProgress):this is specification pattern Must be implemented in the repository 
    //    var assessment = new Assessment
    //    {
    //        PatientId = patientId,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    await unitOfWork.Repository<Assessment>().AddAsync(assessment);
    //    await unitOfWork.CompleteAsync();

    //    var questions = await GetQuestionsAsync();

    //    return new AssessmentDto
    //    {
    //        Id = assessment.Id,
    //        PatientId = assessment.PatientId,
    //        Status = assessment.Status.ToString(),
    //        Questions = questions
    //    };
    //}

    //public async Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync()
    //{
    //    var questionsResponse = new List<QuestionDto>();

    //    var questions = await unitOfWork.Repository<Question>().GetAllAsync();

    //    foreach (var question in questions)
    //    {
    //        var options = new List<string>();

    //        switch (question.AnswerType)
    //        {
    //            case AnswerType.Frequency:
    //                options = Enum.GetNames(typeof(Frequency)).ToList();
    //                break;
    //            case AnswerType.DifficultyLevel:
    //                options = Enum.GetNames(typeof(DifficultyLevel)).ToList();
    //                break;
    //            default:
    //                break;
    //        }

    //        questionsResponse.Add(new QuestionDto
    //        {
    //            Id = question.Id,
    //            Text = question.Text,
    //            Category = question.Category.ToString(),
    //            AnswerType = question.AnswerType.ToString(),
    //            Options = options
    //        });
    //    }

    //    return questionsResponse;
    //}

    public async Task SaveAnswersAsync(int assessmentId, IReadOnlyList<AnswerDto> answers)
    {
        foreach (var a in answers)
        {
            Frequency? freqAnswer = null;
            DifficultyLevel? diffAnswer = null;

            if (Enum.TryParse<Frequency>(a.SelectedOption, out var freq))
            {
                freqAnswer = freq;
            }

            else if (Enum.TryParse<DifficultyLevel>(a.SelectedOption, out var diff))
            {
                diffAnswer = diff;
            }

            var answer = new Answer
            {
                QuestionId = a.QuestionId,
                //FrequencyAnswer = freqAnswer,
                //DifficultyAnswer = diffAnswer
            };

            await unitOfWork.Repository<Answer>().AddAsync(answer);
        }

        await unitOfWork.CompleteAsync();
    }

    public async Task<bool> HasAssessmentAsync(int patientId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
        return assessment is not null;
    }


    public async Task<AssessmentProgressDto> GetAssessmentProgressAsync(int patientId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
        if (assessment != null)
        {
            return new AssessmentProgressDto
            {
                AssessmentId = assessment.Id,
                PatientId = assessment.PatientId,
                Status = assessment.Status,
                CreatedAt = assessment.CreatedAt
            };
        }

        return new AssessmentProgressDto();
    }

    public async Task DeleteAssessmentAsync(int assessmentId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(assessmentId);
        unitOfWork.Repository<Assessment>().Delete(assessment);
        await unitOfWork.CompleteAsync();
    }
}


