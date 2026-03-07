using Rafiq.Application.Dtos.AssessmentDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.EnumExtension;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;
using System.Reflection;

namespace Rafiq.Infrastructure.Services;
public class AssessmentService(IUnitOfWork unitOfWork) : IAssessmentService
{
    public async Task<Result<AssessmentResponseDto>> CreateAssessmentForPatientAsync(int patientId)
    {
        try
        {
            if (patientId <= 0)
            {
                return Result.Failure<AssessmentResponseDto>(400, "Invalid patient ID");
            }

            var patientSpec = new BaseSpecifications<Patient>
            {
                WhereCriteria = p => p.Id == patientId
            };

            var patientResponse = await unitOfWork.Repository<Patient>().GetEntityAsync(patientSpec);

            if(patientResponse == null)
            {
                return Result.Failure<AssessmentResponseDto>(404, "Patient not found");
            }

            var assessmentSpec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.PatientId == patientId
            };

            var assessmentResponse = await unitOfWork.Repository<Assessment>().GetEntityAsync(assessmentSpec);

            if(assessmentResponse != null)
            {
                return Result.Failure<AssessmentResponseDto>(400, "An assessment already exists for this patient");
            }

            var categoriesSpecifications = new BaseSpecifications<Category>();

            categoriesSpecifications.AddIncludeWithThenInclude(a => a.Questions);

            var categories = await unitOfWork.Repository<Category>().GetAllAsync(categoriesSpecifications);

            var assessment = new Assessment
            {
                PatientId = patientId,
                Status = AssessmentStatus.InProgress,
                Categories = categories.Select(category => new AssessmentCategory
                {
                    CategoryId = category.Id,
                    QuestionsNumber = category.QuestionsNumber,
                    CurrentQuestion = 1,
                    Score = 0
                }).ToList()
            };

            await unitOfWork.Repository<Assessment>().AddAsync(assessment);

            await unitOfWork.CompleteAsync();

            var currentAssessmentSpec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.Id == assessment.Id
            };

            currentAssessmentSpec.AddIncludeWithThenInclude(
                a => a.Categories,
                "Category"
            );

            var currentAssessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(currentAssessmentSpec);

            if(currentAssessment == null)
            {
                return Result.Failure<AssessmentResponseDto>(404, "Assessment not found after creation");
            }

            var response = new AssessmentResponseDto
            {
                Id = currentAssessment.Id,
                PatientId = currentAssessment.PatientId,
                Status = currentAssessment.Status.ToString(),
                Categories = currentAssessment.Categories.Select(c => new AssessmentCategoryDto
                {
                    AssessmentCategoryId = c.CategoryId,
                    AssessmentCategoryName = c.Category.Name,
                    AssessmentCategoryDescription = c.Category.Description,
                    AssessmentCurrentQuestion = c.CurrentQuestion,
                    AssessmentTotalQuestions = c.QuestionsNumber,
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
        try
        {   
            if(assessmentId < 0) { 
                return Result.Failure<AssessmentStatusDto>(400, "Invalid assessment ID");
            }

            var spec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.Id == assessmentId
            };

            spec.AddIncludeWithThenInclude(a => a.Categories, "Category", "Category.Questions");

            var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(spec);

            if (assessment == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Assessment not found");
            }

            var totalCategories = assessment.Categories.Count;

            var currentCategory = assessment.Categories.OrderBy(ac => ac.CategoryId)
                .FirstOrDefault(ac => ac.CurrentQuestion <= ac.QuestionsNumber);

            if(currentCategory == null && assessment.Status != AssessmentStatus.Completed)
            {
                return Result.Failure<AssessmentStatusDto>(400, "Assessment is in an invalid state");
            }

            AssessmentCategoryDto? currentCategoryDto = null;

            if (currentCategory != null)
            {
                var question = currentCategory.Category.Questions.FirstOrDefault(q => q.Order == currentCategory.CurrentQuestion);

                currentCategoryDto = new AssessmentCategoryDto
                {
                    AssessmentCategoryId = currentCategory.Id,
                    AssessmentCategoryName = currentCategory.Category.Name,
                    AssessmentCategoryDescription = currentCategory.Category.Description,
                    AssessmentCurrentQuestion = currentCategory.CurrentQuestion,
                    AssessmentTotalQuestions = currentCategory.QuestionsNumber,
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
        catch (Exception ex)
        {
            return Result.Failure<AssessmentStatusDto>(400, ex.ToString());
        }
    }

    public async Task<Result<AssessmentStatusDto>> SubmitAnswerAsync(SubmitAnswerDto dto)
    {
        try
        {
            var spec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.Id == dto.AssessmentId
            };

            spec.AddIncludeWithThenInclude(a => a.Categories, "Category", "Category.Questions");

            var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(spec);

            if (assessment == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Assessment not found");
            }

            if (assessment.Status == AssessmentStatus.Completed)
            {
                return Result.Failure<AssessmentStatusDto>(400, "Assessment already completed");
            }

            var question = await unitOfWork.Repository<Question>().GetEntityAsync(dto.QuestionId);

            if (question == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Question not found");
            }

            var assessmentAnswerSpec = new BaseSpecifications<AssessmentAnswer>
            {
                WhereCriteria = a => a.AssessmentId == dto.AssessmentId && a.QuestionId == dto.QuestionId
            };

            var existingAnswer = await unitOfWork.Repository<AssessmentAnswer>().GetEntityAsync(assessmentAnswerSpec);

            if (existingAnswer != null)
            {
                return Result.Failure<AssessmentStatusDto>(400, "Question already answered");
            }

            var answer = new AssessmentAnswer
            {
                AssessmentId = dto.AssessmentId,
                QuestionId = dto.QuestionId,
                AnswerValue = dto.AnswerValue
            };

            await unitOfWork.Repository<AssessmentAnswer>().AddAsync(answer);

            var assessmentCategory = assessment.Categories.FirstOrDefault(ac => ac.CategoryId == question.CategoryId);

            if (assessmentCategory != null)
            {
                assessmentCategory.Score += question.score;
                assessmentCategory.CurrentQuestion++;
            }

            var allDone = assessment.Categories.All(ac => ac.CurrentQuestion > ac.QuestionsNumber);

            if (allDone) assessment.Status = AssessmentStatus.Completed;

            await unitOfWork.CompleteAsync();

            return await GetAssessmentStatusAsync(dto.AssessmentId);
        }
        catch (Exception ex)
        {
            return Result.Failure<AssessmentStatusDto>(400, ex.ToString());
        }
    }

    private static List<AnswerOptionDto> GetOptionsForAnswerType(AnswerType answerType)
    {
        var member = typeof(AnswerType).GetMember(answerType.ToString()).FirstOrDefault();

        var attr = member?.GetCustomAttribute<AnswerEnumAttribute>();

        if (attr == null) return [];

        return Enum.GetValues(attr.EnumType)
            .Cast<Enum>()
            .Select(e => new AnswerOptionDto
            {
                Value = Convert.ToInt32(e),
                Label = e.GetDisplayName()
            })
            .ToList();
    }
}