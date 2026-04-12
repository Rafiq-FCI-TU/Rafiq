using Rafiq.Domain.Entities;

namespace Rafiq.Application.Interfaces.Repositories;

public interface ISpecialistRepository
{
    Task<IReadOnlyList<SpecialistProfile>> GetAllSpecialistsAsync();
    Task<SpecialistProfile?> GetSpecialistByIdAsync(string specialistId);
    Task<IReadOnlyList<SpecialistPatient>> GetSpecialistPatientsAsync(string specialistId);
    Task<bool> IsPatientAssignedToSpecialistAsync(string specialistId, int patientId);
    Task AssignPatientAsync(string specialistId, int patientId);
    Task UnassignPatientAsync(string specialistId, int patientId);
}