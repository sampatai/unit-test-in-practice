using UnitTestInPractice.Application.Projections;
using UnitTestInPractice.Domain.Root;
using UnitTestInPractice.Infrastructure.Extensions;
namespace UnitTestInPractice.Infrastructure.Repository;

public class ReadOnlyAssessmentRepository(UnitTestInPracticeDbContext UnitTestInPracticeDbContext,
    ILogger<ReadOnlyAssessmentRepository> logger) : IReadOnlyAssessmentRepository
{
    public async Task<(IEnumerable<Assessment> Assessments, int TotalCount)> GetAssessments(FilterModel searchModel, CancellationToken cancellationToken)
    {
        try
        {
            var query = UnitTestInPracticeDbContext.Assessments
            .AsQueryable();


            query = query.WhereIf(!string.IsNullOrEmpty(searchModel.Name), m => m.Name.Contains(searchModel.Name, StringComparison.OrdinalIgnoreCase));
            query = query.WhereIf(searchModel.DateCreated.HasValue, m => m.DateCreated.Date == searchModel.DateCreated!.Value.Date);
            query = query.WhereIf(searchModel.Status != null, m => m.Status== searchModel.Status);

            var totalCount = await query.AsNoTracking().CountAsync(cancellationToken);

            var Assessments = await query.AsNoTracking()
                .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize)
                .ToListAsync(cancellationToken);
            return (Assessments, totalCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{searchModel}", searchModel);
            throw;
        }
    }
    public async Task<bool> HasAssessments(Guid assessmentGuid, CancellationToken cancellationToken)
    {
        try
        {
            return await UnitTestInPracticeDbContext
                        .Assessments
                        .AsNoTracking()
                        .AnyAsync(x => x.AssessmentGUID.Equals(assessmentGuid)
                                , cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{@assessmentGuid}", assessmentGuid);
            throw;
        }
    }

    public async Task<Assessment> GetAsync(Guid AssessmentGuid, CancellationToken cancellationToken)
    {
        try
        {
            return await UnitTestInPracticeDbContext
                 .Assessments
                 .Include(x => x.Responses)
                 .Include(x => x.AssessmentFeedback)
                 .Include(x => x.FinalScore)
                 .FirstOrDefaultAsync(x => x.AssessmentGUID.Equals(AssessmentGuid), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{AssessmentGuid}", AssessmentGuid);
            throw;
        }
    }
}

