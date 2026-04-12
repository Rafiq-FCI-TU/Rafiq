using Rafiq.Application.Dtos.PatientDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IPatientService
{
    Task<Result<IEnumerable<PatientResponseDto>>> GetAllPatientsAsync();
    Task<Result<PatientResponseDto>> GetPatientByIdAsync(int patientId);
}
