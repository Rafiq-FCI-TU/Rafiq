using Rafiq.Domain.Entities;

namespace Rafiq.Application.Dtos.StepsDtos;
public class FamilyStep1Dto
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Phone { get; init; } = null!;
    public string Relationship { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string EmergencyContactName { get; init; } = null!;
    public string EmergencyContactPhone { get; init; } = null!;
}
