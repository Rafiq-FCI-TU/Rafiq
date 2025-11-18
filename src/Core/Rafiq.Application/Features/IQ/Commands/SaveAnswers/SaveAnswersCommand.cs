using MediatR;
using Rafiq.Application.Dtos.IQDtos;

namespace Rafiq.Application.Features.IQ.Commands.SaveAnswers
{
    public record SaveAnswersCommand(int AssessmentId, IReadOnlyList<AnswerDto> Answers) : IRequest<Unit>;
}
