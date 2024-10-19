namespace UnitTestInPractice.Application.Repository;

public interface IAssessmentRepository : IRepository<Assessment>
{
    public Task<Assessment> AddAsync(Assessment  assessment, CancellationToken cancellationToken);
    public Task<Assessment> UpdateAsync(Assessment  assessment, CancellationToken cancellationToken);
    public Task<Assessment> GetAsync(Guid assessmentGuid, CancellationToken cancellationToken);
}

