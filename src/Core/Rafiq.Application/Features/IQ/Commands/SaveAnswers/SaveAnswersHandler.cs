using MediatR;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.IQ.Commands.SaveAnswers
{
    public class SaveAnswersHandler(IIqService _iqService) : IRequestHandler<SaveAnswersCommand, Unit>
    {
        public async Task<Unit> Handle(SaveAnswersCommand request, CancellationToken cancellationToken)
        {
            await _iqService.SaveAnswersAsync(request.AssessmentId, request.Answers);
            return Unit.Value;
        }
    }
}
