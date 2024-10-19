using UnitTestInPractice.Application.Repository;
using UnitTestInPractice.Domain.ValueObjects;

namespace UnitTestInPractice.Application.Command
{
    public static class CreateAssessment
    {
        #region Command/Query
        public sealed record Command : AssessmentCommand, IRequest { }
        #endregion

        #region Validation
        public sealed class Validator : Validator<Command>
        {
            public Validator()
            {

            }
        }
        #endregion
        #region Handler
        public sealed class Handler(ILogger<Handler> logger,
                          IAssessmentRepository assessmentRepository) : IRequestHandler<Command>


        {
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var details = new AssessmentTakerDetails(request.DetailsCommand.FullName, request.DetailsCommand.Occupation, request.DetailsCommand.Address);
                    var assessment = new Assessment(details);
                    request.Responses
                        .ToList()
                        .ForEach(response =>
                        assessment.AddResponse(response.QuestionId, response.Answer));
                    await assessmentRepository.AddAsync(assessment, cancellationToken);
                    await assessmentRepository.UnitOfWork.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{@request}", request);
                    throw;
                }
            }
        }
        #endregion
    }
}
