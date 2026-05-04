using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Application.Dtos;
public class SpecialistPatientDto
{
    public int PatientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
    public string Gender { get; set; } = string.Empty;
    public double TotalScore { get; set; }
    public DateTime AssignedAt { get; set; }
    public string FamilyEmail { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string FamilyId { get; set; } = string.Empty;


}
public class SpecialistResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Credentials { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string ProfessionalBio { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int PatientsCount { get; set; }
}