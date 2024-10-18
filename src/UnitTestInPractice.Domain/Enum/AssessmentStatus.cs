namespace UnitTestInPractice.Domain.Enum;
public class AssessmentStatus : Enumeration
{
    public static AssessmentStatus Pending = new(1, nameof(Pending));
    public static AssessmentStatus Completed = new(2, nameof(Completed));
     public static AssessmentStatus InProgress = new(3, nameof(InProgress));
     public static AssessmentStatus Abandoned = new(4, nameof(Abandoned));
    public AssessmentStatus(int id, string name) : base(id, name)
    {
    }
}

