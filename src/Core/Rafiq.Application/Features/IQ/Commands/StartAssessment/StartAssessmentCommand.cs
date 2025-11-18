using MediatR;
using Rafiq.Application.Dtos.IQDtos;

namespace Rafiq.Application.Features.IQ.Commands.StartAssessment
{
    public record StartAssessmentCommand(int PatientId) : IRequest<AssessmentDto>;
}
