using Rafiq.Application.Dtos.PatientDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Patients;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpecialistRepository _specialistRepository;

    public PatientService(IUnitOfWork unitOfWork, ISpecialistRepository specialistRepository)
    {
        _unitOfWork = unitOfWork;
        _specialistRepository = specialistRepository;
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

    public async Task<Result<PatientDoctorDto>> GetPatientDoctorAsync(int patientId)
    {
        try
        {
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientId);
            if (patient is null)
            {
                return Result.Failure<PatientDoctorDto>(404, "Patient not found");
            }

            var specialistPatient = await _specialistRepository.GetPatientSpecialistAsync(patientId);
            if (specialistPatient is null)
            {
                return Result.Failure<PatientDoctorDto>(404, "Doctor not found for this patient");
            }

            return Result.Success(MapPatientDoctor(specialistPatient));
        }
        catch (Exception ex)
        {
            return Result.Failure<PatientDoctorDto>(500, $"Failed to get patient doctor: {ex.Message}");
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

    private static PatientDoctorDto MapPatientDoctor(SpecialistPatient specialistPatient)
    {
        return new PatientDoctorDto
        {
            Id = specialistPatient.Specialist.Id,
            FullName = $"{specialistPatient.Specialist.FirstName} {specialistPatient.Specialist.LastName}".Trim(),
            Email = specialistPatient.Specialist.Email ?? string.Empty,
            Credentials = specialistPatient.Specialist.Credentials,
            Specialty = specialistPatient.Specialist.Specialty,
            Organization = specialistPatient.Specialist.Organization,
            ProfessionalBio = specialistPatient.Specialist.ProfessionalBio,
            Gender = specialistPatient.Specialist.Gender,
            AssignedAt = specialistPatient.AssignedAt
        };
    }
}
