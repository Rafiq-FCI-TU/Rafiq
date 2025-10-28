using MediatR;
using Rafiq.Application.Dtos.IQDto;
using Rafiq.Application.Interfaces.Services;
namespace Rafiq.Application.Features.IQ.Queries.GetQuestions
{
    public class GetQuestionsHandler(IIQService _iqService): IRequestHandler<GetQuestionsQuery, IReadOnlyList<QuestionDto>>
    {
        public async Task<IReadOnlyList<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _iqService.GetQuestionsAsync();
        }
    }
}
