using Rafiq.Application.Dtos;
using Rafiq.Domain.ErrorHandling;
namespace Rafiq.Application.Interfaces.Services;

    public interface ISpecialistService
    {
        Task<Result<IEnumerable<SpecialistResponseDto>>> GetAllSpecialistsAsync();
        Task<Result<SpecialistResponseDto>> GetSpecialistByIdAsync(string specialistId);
        Task<Result<IEnumerable<SpecialistPatientDto>>> GetSpecialistPatientsAsync(string specialistId);
        Task<Result<bool>> AssignPatientToSpecialistAsync(string specialistId, int patientId);
        Task<Result<bool>> UnassignPatientFromSpecialistAsync(string specialistId, int patientId);
    }