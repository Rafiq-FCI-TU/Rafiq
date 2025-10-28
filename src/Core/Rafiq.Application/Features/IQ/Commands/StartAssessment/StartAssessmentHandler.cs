using MediatR;
using Rafiq.Application.Dtos.IQDto;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.IQ.Commands.StartAssessment
{
    public class StartAssessmentHandler(IIQService _iqService) : IRequestHandler<StartAssessmentCommand, AssessmentDto>
    {
        public async Task<AssessmentDto> Handle(StartAssessmentCommand request, CancellationToken cancellationToken)
        {
            return await _iqService.StartAssessmentAsync(request.PatientId);
        }
    }
}
