namespace UnitTestInPractice.Shared.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
public interface IReadOnlyRepository<T> where T : IAggregateRoot { }