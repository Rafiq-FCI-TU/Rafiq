using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Application.Specifications;

public class AssessmentWithDetailsSpec : BaseSpecifications<Assessment>
{
    public AssessmentWithDetailsSpec(int patientId)
    {
        WhereCriteria = a => a.PatientId == patientId;

        AddInclude("Patient");
        AddInclude("Categories");
        AddInclude("Categories.Category");  // ✅ Full path بدل ThenInclude string
    }
}
