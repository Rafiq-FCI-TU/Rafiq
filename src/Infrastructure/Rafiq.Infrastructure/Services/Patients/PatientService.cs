using Rafiq.Application.Dtos.PatientDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Patients;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<PatientResponseDto>>> GetAllPatientsAsync()
    {
        try
        {
            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();

            var response = patients
                .OrderBy(patient => patient.Id)
                .Select(MapPatient)
                .ToList();

            return Result.Success<IEnumerable<PatientResponseDto>>(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PatientResponseDto>>(500, $"Failed to get patients: {ex.Message}");
        }
    }

    public async Task<Result<PatientResponseDto>> GetPatientByIdAsync(int patientId)
    {
        try
        {
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientId);

            if (patient is null)
            {
                return Result.Failure<PatientResponseDto>(404, "Patient not found");
            }

            return Result.Success(MapPatient(patient));
        }
        catch (Exception ex)
        {
            return Result.Failure<PatientResponseDto>(500, $"Failed to get patient: {ex.Message}");
        }
    }

    private static PatientResponseDto MapPatient(Patient patient)
    {
        return new PatientResponseDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender.ToString(),
            TotalScore = patient.TotalScore,
            FamilyProfileId = patient.FamilyProfileId,
            AssessmentId = patient.AssessmentId
        };
    }
}
