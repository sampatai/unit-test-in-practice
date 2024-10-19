
namespace UnitTestInPractice.Application.Command
{
    public abstract record BaseGUID
    {
        public Guid AssessmentGUID { get; private set; }

    }
    public record AssessmentCommand
    {
        public DetailsCommand  DetailsCommand { get;  set; }
        public IEnumerable<ResponseCommand> Responses { get; set; }

    }
    public record ResponseCommand(Guid QuestionId, string Answer);
    public record DetailsCommand(string FullName, string Occupation, string Address)
}
