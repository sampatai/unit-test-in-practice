
namespace UnitTestInPractice.Infrastructure.Extensions;


public static class IQueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition ? source.Where(predicate).AsQueryable() : source;
    }
}

