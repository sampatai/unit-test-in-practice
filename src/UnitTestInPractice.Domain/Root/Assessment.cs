
namespace UnitTestInPractice.Domain.Root
{
    public class Assessment : Entity, IAggregateRoot
    {
        protected Assessment()
        {

        }

        public Guid AssessmentGUID { get; private set; }
        public string Name { get; private set; }

        public DateTime DateCreated { get; private set; }
        public AssessmentStatus Status { get; private set; }
        public AssessmentTakerDetails Details { get; private set; }
        public Feedback? AssessmentFeedback { get; private set; }

        private List<Response> _responses = new();
        public IReadOnlyCollection<Response> Responses => _responses.AsReadOnly();

        public Assessment(AssessmentTakerDetails assessmentTakerDetails)
        {


            AssessmentGUID = Guid.NewGuid();
            Details = assessmentTakerDetails;
            DateCreated = DateTime.UtcNow;
            Status = AssessmentStatus.Pending;

        }



        public void AddResponse(Guid questionId, string answer)
        {
            Guard.Against.Default(questionId, nameof(questionId));
            Guard.Against.NullOrEmpty(answer, nameof(answer));

            _responses.Add(new Response(questionId, answer));
            Status = AssessmentStatus.InProgress;
        }


        public void CompleteAssessment(int totalQuestion, SeverityLevel severityLevel)
        {
            Status = AssessmentStatus.Completed;
            AssessmentFeedback = GenerateFeedback(severityLevel);
        }



        private Feedback GenerateFeedback(SeverityLevel severity)
        {

            var message = severity.Name switch
            {
                nameof(SeverityLevel.Low) => new FeedbackMessage("You seem to be doing well! Keep up the good work."),
                nameof(SeverityLevel.Moderate) => new FeedbackMessage("You're facing some challenges. Consider reviewing your mental health practices."),
                nameof(SeverityLevel.High) => new FeedbackMessage("You might want to seek professional help soon."),
                nameof(SeverityLevel.Critical) => new FeedbackMessage("Immediate help is recommended. Please contact a professional."),
                _ => new FeedbackMessage("Unknown severity.")
            };

            return new Feedback(AssessmentGUID, severity, message);
        }

    }
}
