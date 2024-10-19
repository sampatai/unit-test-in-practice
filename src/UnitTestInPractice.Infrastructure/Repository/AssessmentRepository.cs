using UnitTestInPractice.Application.Repository;
using UnitTestInPractice.Domain.Root;

namespace UnitTestInPractice.Infrastructure.Repository;

public class AssessmentRepository(UnitTestInPracticeDbContext UnitTestInPracticeDbContext,
    ILogger<AssessmentRepository> logger) : IAssessmentRepository
{
    public IUnitOfWork UnitOfWork => UnitTestInPracticeDbContext;

    public async Task<Assessment> AddAsync(Assessment Assessments, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await UnitTestInPracticeDbContext.Assessments.AddAsync(Assessments);
            return entity.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{Assessments}", Assessments);
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
                  .Include(x=>x.Details)
                  .FirstOrDefaultAsync(x => x.AssessmentGUID.Equals(AssessmentGuid), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{AssessmentGuid}", AssessmentGuid);
            throw;
        }
    }

    public async Task<Assessment> UpdateAsync(Assessment Assessments, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await Task.FromResult(UnitTestInPracticeDbContext.Assessments.Update(Assessments));
            return entity.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "@{Assessments}", Assessments);
            throw;
        }
    }
}

