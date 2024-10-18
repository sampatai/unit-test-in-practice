namespace UnitTestInPractice.Domain.Entities
{
    public class Feedback
    {
        protected Feedback()
        {
            
        }
        public Guid FeedbackGUID { get; private set; }
        public Guid AssessmentGUID { get; private set; }
        public SeverityLevel Severity { get; private set; }
        public FeedbackMessage Message { get; private set; }

        public Feedback(Guid assessmentId, SeverityLevel severity, FeedbackMessage message)
        {
            FeedbackGUID = Guid.NewGuid();
            Guard.Against.Default(assessmentId, nameof(assessmentId));
            Guard.Against.Null(message, nameof(message));
            Severity = severity;
            Message = message;
        }
    }
}
