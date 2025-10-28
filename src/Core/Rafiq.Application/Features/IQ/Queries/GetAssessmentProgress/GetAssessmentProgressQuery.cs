using MediatR;
using Rafiq.Application.Dtos.IQDto;

namespace Rafiq.Application.Features.IQ.Queries.GetAssessmentProgress
{
    public record GetAssessmentProgressQuery(int PatientId) : IRequest<AssessmentProgressDto>;
}
