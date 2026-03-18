using Rafiq.Domain.ErrorHandling;
namespace Rafiq.Application.Interfaces.Services
{
    public interface IPatientSessionAttemptService
    {
        Task<Result<int>> SubmitAttemptAsync(SubmitAttemptDto dto);
        Task<Result<bool>> ReviewAttemptAsync(ReviewAttemptDto dto);
        Task<Result<IEnumerable<AttemptDto>>> GetAttemptsByPatientAsync(int patientId);
        Task<Result<IEnumerable<AttemptDto>>> GetPendingAttemptsForSpecialistAsync(string specialistId);
    }
}
