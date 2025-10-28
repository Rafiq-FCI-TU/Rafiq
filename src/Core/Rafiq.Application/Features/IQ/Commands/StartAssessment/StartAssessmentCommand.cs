using MediatR;
using Rafiq.Application.Dtos.IQDto;

namespace Rafiq.Application.Features.IQ.Commands.StartAssessment
{
    public record StartAssessmentCommand(int PatientId) : IRequest<AssessmentDto>;
}
