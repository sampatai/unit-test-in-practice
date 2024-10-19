

using UnitTestInPractice.Application.Projections;

namespace UnitTestInPractice.Application.Repository;

public interface IReadOnlyAssessmentRepository : IReadOnlyRepository<Assessment>
{
    public Task<(IEnumerable<Assessment> Assessments, int TotalCount)> GetAssessments(FilterModel searchModel,CancellationToken cancellationToken);
    public Task<bool> HasAssessments(Guid assessmentGuid,CancellationToken cancellationToken);
    Task<Assessment> GetAsync(Guid assessmentGuid, CancellationToken cancellationToken);
}

