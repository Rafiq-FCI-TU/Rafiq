using MediatR;
using Rafiq.Application.Interfaces.Services;
namespace Rafiq.Application.Features.IQ.Commands.DeleteAssessment
{
    public class DeleteAssessmentHandler : IRequestHandler<DeleteAssessmentCommand, Unit>
    {
        private readonly IIqService _iqService;

        public DeleteAssessmentHandler(IIqService iqService)
        {
            _iqService = iqService;
        }

        public async Task<Unit> Handle(DeleteAssessmentCommand request, CancellationToken cancellationToken)
        {
            await _iqService.DeleteAssessmentAsync(request.AssessmentId);
            return Unit.Value;
        }
    }
}
