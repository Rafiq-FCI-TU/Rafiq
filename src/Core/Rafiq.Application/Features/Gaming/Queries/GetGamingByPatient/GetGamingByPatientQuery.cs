using MediatR;
using Rafiq.Application.Dtos.GamingDtos;

namespace Rafiq.Application.Features.Gaming.Queries.GetGamingByPatient;
public record GetGamingByPatientQuery(int PatientId) : IRequest<GamingDto>;
