using MediatR;
using Rafiq.Application.Dtos.IQDtos;

namespace Rafiq.Application.Features.IQ.Queries.GetAssessmentProgress
{
    public record GetAssessmentProgressQuery(int PatientId) : IRequest<AssessmentProgressDto>;
}
