namespace UnitTestInPractice.Domain.Enum;

public class SeverityLevel : Enumeration
{
    public static SeverityLevel Low = new(1, nameof(Low));
    public static SeverityLevel Moderate = new(2, nameof(Moderate));
    public static SeverityLevel High = new(3, nameof(High));
    public static SeverityLevel Critical = new(4, nameof(Critical));
    public SeverityLevel(int id, string name) : base(id, name)
    {
    }
}