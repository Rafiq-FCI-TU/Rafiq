using MediatR;
namespace Rafiq.Application.Features.IQ.Commands.DeleteAssessment
{
    public record DeleteAssessmentCommand(int AssessmentId) : IRequest<Unit>;
}
