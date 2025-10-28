using MediatR;
using Rafiq.Application.Dtos.IQDto;

namespace Rafiq.Application.Features.IQ.Commands.SaveAnswers
{
    public record SaveAnswersCommand(int AssessmentId, IReadOnlyList<AnswerDto> Answers) : IRequest<Unit>;
}
