using MediatR;
using Rafiq.Application.Dtos.IQDtos;

namespace Rafiq.Application.Features.IQ.Queries.GetQuestions
{
    public record GetQuestionsQuery() : IRequest<IReadOnlyList<QuestionDto>>;
}
