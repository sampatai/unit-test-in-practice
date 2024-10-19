using UnitTestInPractice.Application.Repository;
using UnitTestInPractice.Domain.Enum;
using UnitTestInPractice.Domain.ValueObjects;

namespace UnitTestInPractice.Application.Query;

public static class GetAssessments
{
    #region Request
    public record Query : FilterModel, IRequest<ListAssessment>
    {

    }
    #endregion
    #region Validation
    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {

            RuleFor(x => x.Status)
                 .Must(x => Enumeration.GetAll<AssessmentStatus>().Any(a => a.Id == x.Id))
                 .When(x => x.Status is not null && x.Status.Id > 0)
                .WithMessage("Invalid status.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }

    #endregion
    #region Handler
    public sealed class Handler(ILogger<Handler> logger,
            IReadOnlyAssessmentRepository assessmentRepository) : IRequestHandler<Query, ListAssessment>
    {
        public async Task<ListAssessment> Handle(Query request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await assessmentRepository.GetAssessments(request, cancellationToken);
                return new ListAssessment(result.Assessments
                    .Select(x => new Assements(x.Details.FullName, x.Status, x.AssessmentFeedback?.Message.Message, x.DateCreated, x.AssessmentGUID)), result.TotalCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "@{request}", request);
                throw;
            }
        }
    }
    #endregion
    public record ListAssessment(IEnumerable<Assements> Assements, int TotalCount);

    public record Assements(string FullName, AssessmentStatus Status, string? FeedbackMessage, DateTime CreatedDate, Guid AsessmentGuid);
}
