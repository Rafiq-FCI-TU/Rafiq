using AutoMapper;
using Rafiq.Application.Dtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Specialists;

public class SpecialistService : ISpecialistService
{
    private readonly ISpecialistRepository _specialistRepository;
    private readonly IMapper _mapper;

    public SpecialistService(ISpecialistRepository specialistRepository, IMapper mapper)
    {
        _specialistRepository = specialistRepository;
        _mapper = mapper;
    }

    #region Get All Specialists

    public async Task<Result<IEnumerable<SpecialistResponseDto>>> GetAllSpecialistsAsync()
    {
        try
        {
            var specialists = await _specialistRepository.GetAllSpecialistsAsync();
            var dtos = _mapper.Map<IEnumerable<SpecialistResponseDto>>(specialists);

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<SpecialistResponseDto>>(500, $"Failed to get specialists: {ex.Message}");
        }
    }

    #endregion

    #region Get Specialist By Id

    public async Task<Result<SpecialistResponseDto>> GetSpecialistByIdAsync(string specialistId)
    {
        try
        {
            var specialist = await _specialistRepository.GetSpecialistByIdAsync(specialistId);

            if (specialist is null)
                return Result.Failure<SpecialistResponseDto>(404, "Specialist not found");

            return Result.Success(_mapper.Map<SpecialistResponseDto>(specialist));
        }
        catch (Exception ex)
        {
            return Result.Failure<SpecialistResponseDto>(500, $"Failed to get specialist: {ex.Message}");
        }
    }

    #endregion

    #region Get Specialist Patients

    public async Task<Result<IEnumerable<SpecialistPatientDto>>> GetSpecialistPatientsAsync(string specialistId)
    {
        try
        {
            var specialist = await _specialistRepository.GetSpecialistByIdAsync(specialistId);

            if (specialist is null)
                return Result.Failure<IEnumerable<SpecialistPatientDto>>(404, "Specialist not found");

            var specialistPatients = await _specialistRepository.GetSpecialistPatientsAsync(specialistId);

            if (!specialistPatients.Any())
                return Result.Failure<IEnumerable<SpecialistPatientDto>>(404, "No patients found for this specialist");

            var dtos = _mapper.Map<IEnumerable<SpecialistPatientDto>>(specialistPatients);

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<SpecialistPatientDto>>(500, $"Failed to get patients: {ex.Message}");
        }
    }

    #endregion

    #region Assign / Unassign Patient

    public async Task<Result<bool>> AssignPatientToSpecialistAsync(string specialistId, int patientId)
    {
        try
        {
            var alreadyAssigned = await _specialistRepository
                .IsPatientAssignedToSpecialistAsync(specialistId, patientId);

            if (alreadyAssigned)
                return Result.Failure<bool>(409, "Patient is already assigned to this specialist");

            await _specialistRepository.AssignPatientAsync(specialistId, patientId);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(500, $"Failed to assign patient: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UnassignPatientFromSpecialistAsync(string specialistId, int patientId)
    {
        try
        {
            var isAssigned = await _specialistRepository
                .IsPatientAssignedToSpecialistAsync(specialistId, patientId);

            if (!isAssigned)
                return Result.Failure<bool>(404, "Patient is not assigned to this specialist");

            await _specialistRepository.UnassignPatientAsync(specialistId, patientId);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(500, $"Failed to unassign patient: {ex.Message}");
        }
    }

    #endregion
}