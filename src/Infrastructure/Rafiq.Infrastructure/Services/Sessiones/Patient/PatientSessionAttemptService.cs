using AutoMapper;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Sessiones;

public class PatientSessionAttemptService : IPatientSessionAttemptService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly BlobHelperService _blobHelper;

    public PatientSessionAttemptService(IUnitOfWork unitOfWork,IMapper mapper,BlobHelperService blobHelper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobHelper = blobHelper;
    }

    #region Patient Session Attempt Operations

    public async Task<Result<int>> SubmitAttemptAsync(SubmitAttemptDto dto)
    {
        var blockingAttempt = await GetBlockingAttemptAsync(dto.PatientId, dto.SessionId);
        if (blockingAttempt != null)
            return Result.Failure<int>(400, GetBlockingReason(blockingAttempt.Status));

        var attempt = await BuildAttemptAsync(dto);

        await _unitOfWork.Repository<PatientSessionAttempt>().AddAsync(attempt);
        await _unitOfWork.CompleteAsync();

        return Result.Success(attempt.Id);
    }

    public async Task<Result<bool>> ReviewAttemptAsync(ReviewAttemptDto dto)
    {
        var attempt = await GetAttemptWithDetailsAsync(dto.AttemptId);
        if (attempt == null)
            return Result.Failure<bool>(404, "Attempt not found");

        var status = ParseStatus(dto.Status);

        ApplyReview(attempt, status, dto);

        if (IsApprovedWithScore(status, dto.ScoreAwarded))
            UpdatePatientScore(attempt, dto.ScoreAwarded!.Value);

        _unitOfWork.Repository<PatientSessionAttempt>().Update(attempt);
        await _unitOfWork.CompleteAsync();

        return Result.Success(true);
    }

    public async Task<Result<IEnumerable<AttemptDto>>> GetPendingAttemptsForSpecialistAsync(string specialistId)
    {
        var spec = new BaseSpecifications<PatientSessionAttempt>
        {
            WhereCriteria = a =>
                a.Session.SpecialistProfileId == specialistId &&
                a.Status == AttemptStatus.PendingReview
        };
        spec.AddInclude("Session");
        spec.AddInclude("Patient");

        var attempts = await _unitOfWork.Repository<PatientSessionAttempt>().GetAllAsync(spec);
        return Result.Success(_mapper.Map<IEnumerable<AttemptDto>>(attempts));
    }

    public async Task<Result<IEnumerable<AttemptDto>>> GetAttemptsByPatientAsync(int patientId)
    {
        var spec = new BaseSpecifications<PatientSessionAttempt>
        {
            WhereCriteria = a => a.PatientId == patientId
        };
        spec.AddInclude("Session");

        var attempts = await _unitOfWork.Repository<PatientSessionAttempt>().GetAllAsync(spec);
        return Result.Success(_mapper.Map<IEnumerable<AttemptDto>>(attempts));
    }

    #endregion

    #region Query Helpers

    private async Task<PatientSessionAttempt?> GetBlockingAttemptAsync(int patientId, int sessionId)
    {
        var spec = new BaseSpecifications<PatientSessionAttempt>
        {
            WhereCriteria = a =>
                a.PatientId == patientId &&
                a.SessionId == sessionId &&
                (a.Status == AttemptStatus.PendingReview || a.Status == AttemptStatus.Approved)
        };
        return await _unitOfWork.Repository<PatientSessionAttempt>().GetEntityAsync(spec);
    }

    private async Task<PatientSessionAttempt?> GetAttemptWithDetailsAsync(int attemptId)
    {
        var spec = new BaseSpecifications<PatientSessionAttempt>
        {
            WhereCriteria = a => a.Id == attemptId
        };
        spec.AddInclude("Patient");
        spec.AddInclude("Session");

        return await _unitOfWork.Repository<PatientSessionAttempt>().GetEntityAsync(spec);
    }

    #endregion

    #region Build Helpers

    private async Task<PatientSessionAttempt> BuildAttemptAsync(SubmitAttemptDto dto)
    {
        var videoUrl = await _blobHelper.UploadFileAsync(dto.Video, "patient-attempts");
        var duration = await _blobHelper.GetVideoDuration(dto.Video);

        return new PatientSessionAttempt
        {
            SessionId    = dto.SessionId,
            PatientId    = dto.PatientId,
            VideoUrl     = videoUrl,
            Duration     = duration,
            Status       = AttemptStatus.PendingReview,
            SubmittedAt  = DateTime.UtcNow
        };
    }

    #endregion

    #region Review Helpers

    private static void ApplyReview(PatientSessionAttempt attempt, AttemptStatus status, ReviewAttemptDto dto)
    {
        attempt.Status         = status;
        attempt.DoctorFeedback = dto.DoctorFeedback;
        attempt.ReviewedAt     = DateTime.UtcNow;
    }

    private static void UpdatePatientScore(PatientSessionAttempt attempt, int scoreAwarded)
    {
        attempt.ScoreAwarded          = scoreAwarded;
        attempt.Patient.TotalScore   += scoreAwarded;
    }

    private static bool IsApprovedWithScore(AttemptStatus status, int? scoreAwarded)
        => status == AttemptStatus.Approved && scoreAwarded.HasValue;

    #endregion

    #region Validation Helpers

    private static string GetBlockingReason(AttemptStatus status)
        => status == AttemptStatus.PendingReview
            ? "You already have an attempt pending review"
            : "This session is already approved";

    private static AttemptStatus ParseStatus(string status)
    {
        if (!Enum.TryParse<AttemptStatus>(status, ignoreCase: true, out var result))
            throw new ArgumentException($"Invalid status value: {status}");
        return result;
    }

    #endregion
}