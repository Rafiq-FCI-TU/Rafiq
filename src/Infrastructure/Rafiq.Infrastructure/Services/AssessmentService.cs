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

            // Check if patient exists
            if (patientResponse == null)
            {
                return Result.Failure<AssessmentResponseDto>(404, "Patient not found");
            }

            var assessmentSpec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.PatientId == patientId
            };

            // Check if an assessment already exists for the patient
            var assessmentResponse = await unitOfWork.Repository<Assessment>().GetEntityAsync(assessmentSpec);

            if(assessmentResponse != null)
            {
                return Result.Failure<AssessmentResponseDto>(400, "An assessment already exists for this patient");
            }

            var categoriesSpecifications = new BaseSpecifications<Category>();

            categoriesSpecifications.AddIncludeWithThenInclude(a => a.Questions);

            // Get all categories with their questions to create the assessment categories
            var categories = await unitOfWork.Repository<Category>().GetAllAsync(categoriesSpecifications);

            var assessment = new Assessment
            {
                PatientId = patientId,
                Status = AssessmentStatus.InProgress,
                Categories = categories.Select(category => new PracticeCategory
                {
                    CategoryId = category.Id,
                    QuestionsNumber = category.QuestionsNumber,
                    CurrentQuestion = 1,
                    QuestionsEarnedPoints = 0,
                    QuestionsMaxPoints = 0,
                    Score = 0
                }).ToList()
            };

            await unitOfWork.Repository<Assessment>().AddAsync(assessment);

            await unitOfWork.CompleteAsync();

            var currentAssessmentSpec = new BaseSpecifications<Assessment>
            {
                WhereCriteria = a => a.Id == assessment.Id
            };

            patientResponse.AssessmentId = assessment.Id;

            await unitOfWork.CompleteAsync();

            currentAssessmentSpec.AddIncludeWithThenInclude(
                a => a.Categories,
                "Category"
            );

            var currentAssessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(currentAssessmentSpec);

            // Check if the assessment was created successfully
            if (currentAssessment == null)
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
                    QuestionsEarnedPoints = c.QuestionsEarnedPoints,
                    QuestionsMaxPoints = c.QuestionsMaxPoints,
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

            // Check if the assessment exists
            if (assessment == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Assessment not found");
            }

            var totalCategories = assessment.Categories.Count;

            var currentCategory = assessment.Categories.OrderBy(ac => ac.CategoryId)
                .FirstOrDefault(ac => ac.CurrentQuestion <= ac.QuestionsNumber);

            // If there is no current category and the assessment is not completed, it means the assessment is in an invalid state
            if (currentCategory == null && assessment.Status != AssessmentStatus.Completed)
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
                    QuestionsEarnedPoints = currentCategory.QuestionsEarnedPoints,
                    QuestionsMaxPoints = currentCategory.QuestionsMaxPoints,
                    Score = currentCategory.Score,
                    Question = question == null ? null : new QuestionDto
                    {
                        QuestionId = question.Id,
                        Text = question.Text,
                        MaxPoints = question.MaxPoints,
                        Options = GetOptions()
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

            // Check if the assessment exists
            if (assessment == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Assessment not found");
            }

            // Check if the assessment is already completed
            if (assessment.Status == AssessmentStatus.Completed)
            {
                return Result.Failure<AssessmentStatusDto>(400, "Assessment already completed");
            }

            var question = await unitOfWork.Repository<Question>().GetEntityAsync(dto.QuestionId);

            // Check if the question exists
            if (question == null)
            {
                return Result.Failure<AssessmentStatusDto>(404, "Question not found");
            }

            var assessmentAnswerSpec = new BaseSpecifications<AssessmentAnswer>
            {
                WhereCriteria = a => a.AssessmentId == dto.AssessmentId && a.QuestionId == dto.QuestionId
            };

            var existingAnswer = await unitOfWork.Repository<AssessmentAnswer>().GetEntityAsync(assessmentAnswerSpec);

            // Check if the question has already been answered for this assessment
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

            // Update the assessment category score and current question
            var assessmentCategory = assessment.Categories.FirstOrDefault(ac => ac.CategoryId == question.CategoryId);

            if (assessmentCategory != null)
            {
                AnswerType answerType = (AnswerType)dto.AnswerValue;

                assessmentCategory.QuestionsEarnedPoints += dto.AnswerValue * 5; // 0 -> 0 points, 1 -> 5 points, 2 -> 10 points
                assessmentCategory.QuestionsMaxPoints = assessmentCategory.CurrentQuestion * 10; // Each question is worth 10 points
                assessmentCategory.Score = (double)assessmentCategory.QuestionsEarnedPoints / (double)assessmentCategory.QuestionsMaxPoints * 100; // Calculate the score as a percentage

                assessmentCategory.CurrentQuestion++;
            }

            var allDone = assessment.Categories.All(ac => ac.CurrentQuestion > ac.QuestionsNumber);

            if (allDone)
            {
                assessment.Status = AssessmentStatus.Completed;

                var categoriesCount = assessment.Categories.Count;

                var categoriesScore = 0.0;

                foreach (var category in assessment.Categories) {
                    categoriesScore += category.Score;
                }

                var iq = categoriesScore / categoriesCount;

                var patientSpecs = new BaseSpecifications<Patient>()
                {
                    WhereCriteria = p => p.Id == assessment.PatientId
                };

                var patient = await unitOfWork.Repository<Patient>().GetEntityAsync(patientSpecs);

                if(patient is not null)
                {
                    patient.TotalScore = iq;
                }
            }

            await unitOfWork.CompleteAsync();

            return await GetAssessmentStatusAsync(dto.AssessmentId);
        }
        catch (Exception ex)
        {
            return Result.Failure<AssessmentStatusDto>(400, ex.ToString());
        }
    }

    private static List<AnswerOptionDto> GetOptions()
    {
        return [.. Enum.GetValues(typeof(AnswerType))
            .Cast<AnswerType>()
            .Select(e => new AnswerOptionDto
            {
                Value = (int)e,
                Label = e.GetDisplayName()
            })];
    }
}