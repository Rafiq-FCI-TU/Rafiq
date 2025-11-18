namespace Rafiq.Application.Dtos;
public record FamilyStep1Dto(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string Relationship,
    string Address,
    string EmergencyContactName,
    string EmergencyContactPhone
);
