using MediatR;
using Rafiq.Application.Dtos.IQDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.IQ.Queries.GetAssessmentProgress
{
    public class GetAssessmentProgressHandler(IIqService _iqService) : IRequestHandler<GetAssessmentProgressQuery, AssessmentProgressDto>
    {
        public async Task<AssessmentProgressDto> Handle(GetAssessmentProgressQuery request, CancellationToken cancellationToken)
        {
            return await _iqService.GetAssessmentProgressAsync(request.PatientId);
        }
    }
}
