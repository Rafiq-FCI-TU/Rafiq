using MediatR;
using Rafiq.Application.Dtos.IQDto;
namespace Rafiq.Application.Features.IQ.Queries.GetQuestions
{
    public record GetQuestionsQuery() : IRequest<IReadOnlyList<QuestionDto>>;
}
