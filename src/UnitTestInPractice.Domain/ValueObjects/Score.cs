

namespace UnitTestInPractice.Domain.ValueObjects;
public class Score : ValueObject
{
    protected Score()
    {

    }
    public int Value { get; }

    public Score(int value)
    {
        Guard.Against.OutOfRange(value, nameof(value), 0, 100);
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
       
    }
}

