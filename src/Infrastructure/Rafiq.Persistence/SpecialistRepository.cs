using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Persistence.Data;

namespace Rafiq.Persistence;

public class SpecialistRepository : ISpecialistRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RafiqDbContext _dbContext;

    public SpecialistRepository(IUnitOfWork unitOfWork, RafiqDbContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    #region Get All Specialists

    public async Task<IReadOnlyList<SpecialistProfile>> GetAllSpecialistsAsync()
    {
        return await _dbContext.Set<SpecialistProfile>()
            .AsNoTracking()
            .Include(s => s.SpecialistPatients)
            .ToListAsync();
    }

    #endregion

    #region Get Specialist By Id

    public async Task<SpecialistProfile?> GetSpecialistByIdAsync(string specialistId)
    {
        return await _dbContext.Set<SpecialistProfile>()
            .AsNoTracking()
            .Include(s => s.SpecialistPatients)
                .ThenInclude(sp => sp.Patient)
                    .ThenInclude(p => p.FamilyProfile)
            .FirstOrDefaultAsync(s => s.Id == specialistId);
    }

    #endregion

    #region Get Specialist Patients

    public async Task<IReadOnlyList<SpecialistPatient>> GetSpecialistPatientsAsync(string specialistId)
    {
        return await _dbContext.Set<SpecialistPatient>()
            .AsNoTracking()
            .Where(sp => sp.SpecialistId == specialistId)
            .Include(sp => sp.Patient)
                .ThenInclude(p => p.FamilyProfile)
            .ToListAsync();
    }

    #endregion

    #region Check Assignment

    public async Task<bool> IsPatientAssignedToSpecialistAsync(string specialistId, int patientId)
    {
        return await _dbContext.Set<SpecialistPatient>()
            .AnyAsync(sp => sp.SpecialistId == specialistId && sp.PatientId == patientId);
    }

    #endregion

    #region Assign Patient

    public async Task AssignPatientAsync(string specialistId, int patientId)
    {
        var patientSpec = new BaseSpecifications<Patient>
        {
            WhereCriteria = p => p.Id == patientId
        };

        var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientSpec);
        if (patient is null)
            throw new Exception($"Patient with id '{patientId}' not found.");

        var specialistPatient = new SpecialistPatient
        {
            SpecialistId = specialistId,
            PatientId = patientId,
            AssignedAt = DateTime.UtcNow
        };

        await _dbContext.Set<SpecialistPatient>().AddAsync(specialistPatient);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region Unassign Patient

    public async Task UnassignPatientAsync(string specialistId, int patientId)
    {
        var record = await _dbContext.Set<SpecialistPatient>()
            .FirstOrDefaultAsync(sp => sp.SpecialistId == specialistId && sp.PatientId == patientId);

        if (record is not null)
        {
            _dbContext.Set<SpecialistPatient>().Remove(record);
            await _dbContext.SaveChangesAsync();
        }
    }

    #endregion
}