namespace Rafiq.Application.Dtos;
public record PatientStep2Dto(
    string Token,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender
);